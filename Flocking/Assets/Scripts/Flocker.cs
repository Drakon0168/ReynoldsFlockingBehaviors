using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Agent {

    [SerializeField]
    private float height, visionRange;
    [HideInInspector]
    public float cohesionWeight, separationWeight, alignmentWeight, seekWeight;

    private void Start()
    {
        Flock.Flockers.Add(this);
    }

    protected override void Update()
    {
        velocity += (Cohesion() * cohesionWeight + Separation(visionRange) * separationWeight + Alignment() * alignmentWeight + Seek(Flock.SeekPoint) * seekWeight).normalized * Time.deltaTime * moveSpeed;

        base.Update();

        StayOnTerrain();
    }

    /// <summary>
    /// Moves this unit towards the center of the flock
    /// </summary>
    /// <returns></returns>
    protected Vector3 Cohesion()
    {
        return Seek(Flock.CenterPosition);
    }

    /// <summary>
    /// Moves this unit away from the other units surrounding it
    /// </summary>
    /// <param name="radius">The distance to start separating from other units</param>
    /// <returns></returns>
    protected Vector3 Separation(float radius)
    {
        Vector3 fleeDirection = Vector3.zero;
        int fleeCount = 0;

        foreach(Flocker flocker in Flock.Flockers)
        {
            if((flocker.transform.position - transform.position).sqrMagnitude < radius * radius)
            {
                fleeDirection += Flee(flocker.transform.position);
                fleeCount++;
            }
        }

        if(fleeCount > 0)
        {
            fleeDirection /= fleeCount;
        }

        return fleeDirection.normalized;
    }

    /// <summary>
    /// Moves this unit in the same direction as the others around it
    /// </summary>
    /// <returns></returns>
    private Vector3 Alignment()
    {
        return (Flock.FlockAlignment - velocity).normalized;
    }

    private void StayOnTerrain()
    {
        float height = this.height;
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * 20, Vector3.down);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Terrain")
            {
                height += hit.point.y;
                break;
            }
        }

        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}

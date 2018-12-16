using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

    protected Vector3 velocity, acceleration;
    [SerializeField]
    protected float moveSpeed;
    
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    /// <summary>
    /// Returns to direction to apply force in order to move towards a point
    /// </summary>
    protected Vector3 Seek(Vector3 target)
    {
        Vector3 targetVelocity = (target - transform.position).normalized * moveSpeed;

        return (targetVelocity - velocity).normalized;
    }

    /// <summary>
    /// Returns the direction to apply force in order to flee a point weighted by the distance from the point
    /// </summary>
    protected Vector3 Flee(Vector3 target)
    {
        Vector3 targetVelocity = (transform.position - target).normalized;
        return targetVelocity;
    }

    protected virtual void Update()
    {
        velocity += acceleration;
        transform.rotation = Quaternion.Euler(Mathf.Asin(velocity.y / velocity.magnitude) * Mathf.Rad2Deg * -1, Mathf.Atan2(velocity.z * -1, velocity.x) * Mathf.Rad2Deg + 90, 0);
        transform.position += velocity * Time.deltaTime;
    }
}

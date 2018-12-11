using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    private Vector3 from, to, seekPoint;
    private Vector3 velocity;
    [SerializeField]
    private float moveSpeed;
    private int currentIndex;

	void Start () {
        currentIndex = Mathf.FloorToInt(Random.value * PathGenerator.Points.Count);

        if(currentIndex == 0)
        {
            from = PathGenerator.Points[PathGenerator.Points.Count - 1];
        }
        else
        {
            from = PathGenerator.Points[currentIndex - 1];
        }

        to = PathGenerator.Points[currentIndex];
        transform.position = from;

        velocity = (to - from).normalized * moveSpeed;
	}
	
	void Update () {
        Vector3 path = to - from;
        float seekMult = Vector3.Dot(path, (transform.position + velocity) - from) / Vector3.Dot(path, path);

        if(seekMult <= 1)
        {
            if(seekMult < 0)
            {
                seekMult = 0;
            }
            
            velocity += Seek(from + path * seekMult) * Time.deltaTime;
        }
        else if(seekMult > 0)
        {
            GetNextPoint();
        }

        transform.rotation = Quaternion.Euler(Mathf.Asin(velocity.y / velocity.magnitude) * Mathf.Rad2Deg * -1, Mathf.Atan2(velocity.z * -1, velocity.x) * Mathf.Rad2Deg + 90, 0);
        transform.position += velocity * Time.deltaTime;
	}

    /// <summary>
    /// Returns to direction to apply force in order to move towards a point
    /// </summary>
    /// <returns></returns>
    private Vector3 Seek(Vector3 target)
    {
        Vector3 targetVelocity = (target - transform.position).normalized * moveSpeed;

        return targetVelocity - velocity;
    }

    /// <summary>
    /// Sets from and to to the current and next points along the path
    /// </summary>
    private void GetNextPoint()
    {
        from = to;
        currentIndex++;

        if(currentIndex >= PathGenerator.Points.Count)
        {
            currentIndex = 0;
        }

        to = PathGenerator.Points[currentIndex];
    }
}

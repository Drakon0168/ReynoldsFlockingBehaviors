using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : Agent {

    private Vector3 from, to, seekPoint;
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
	
	protected override void Update () {
        Vector3 path = to - from;
        float seekMult = Vector3.Dot(path, (transform.position + velocity) - from) / Vector3.Dot(path, path);

        if(seekMult <= 1)
        {
            if(seekMult < 0)
            {
                seekMult = 0;
            }
            
            velocity += Seek(from + path * seekMult) * moveSpeed * Time.deltaTime;
        }
        else if(seekMult > 0)
        {
            GetNextPoint();
        }

        base.Update();
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

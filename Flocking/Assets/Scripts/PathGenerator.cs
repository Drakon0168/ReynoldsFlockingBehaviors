using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour {

    [SerializeField]
    private int numPoints;
    [SerializeField]
    private Material GLMaterial;
    private List<Vector3> points;
    [SerializeField]
    private float minDistance, maxDistance, minHeight, maxHeight;

    private void Start()
    {
        points = new List<Vector3>();

        GeneratePath();
    }

    private void Update () {
		
	}

    private void GeneratePath()
    {
        float angleChange = (Mathf.PI * 2) / numPoints;

        for(int i = 0; i < numPoints; i++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(angleChange * i), Mathf.Sin(angleChange * i));

            Vector2 xzPoint = direction * (Random.value * (maxDistance - minDistance) + minDistance);
            float height = Random.value * (maxHeight - minHeight) + minHeight;

            points.Add(new Vector3(xzPoint.x, height, xzPoint.y));
        }

        //for(int i = 0; i < points.Count; i++)
        //{
        //    if(i == 0)
        //    {
        //        Debug.DrawLine(points[i], points[points.Count - 1], Color.red, 60f, false);
        //    }
        //    else
        //    {
        //        Debug.DrawLine(points[i], points[i - 1], Color.red, 60f, false);
        //    }
        //}
    }

    private void OnRenderObject()
    {
        if(points.Count > 0)
        {
            GLMaterial.SetPass(0);

            GL.Color(Color.red);
            GL.Begin(GL.LINES);

            foreach (Vector3 position in points)
            {
                GL.Vertex(position);
            }

            GL.End();
        }
    }
}

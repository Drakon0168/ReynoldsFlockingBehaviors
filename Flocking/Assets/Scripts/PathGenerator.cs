using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour {

    [SerializeField]
    private int numPoints;
    [SerializeField]
    private Material GLMaterial;
    private static List<Vector3> points;
    [SerializeField]
    private float minDistance, maxDistance, minHeight, maxHeight;

    static public List<Vector3> Points
    {
        get { return points; }
    }

    private void Start()
    {
        points = new List<Vector3>();
        TerrainGenerator.reset += Reset;

        Reset();
    }

    private void Reset()
    {
        if(points != null)
        {
            points.Clear();
        }

        GeneratePath();
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
    }

    private void OnRenderObject()
    {
        if(points.Count > 0)
        {
            GLMaterial.SetPass(0);
            
            GL.Color(Color.red);
            GL.Begin(GL.LINES);

            for(int i = 0; i < points.Count; i++)
            {
                if(i == 0)
                {
                    GL.Vertex(points[i]);
                    GL.Vertex(points[points.Count - 1]);
                }
                else
                {
                    GL.Vertex(points[i]);
                    GL.Vertex(points[i - 1]);
                }
            }

            GL.End();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour {

    [SerializeField]
    private int numPoints;
    [SerializeField]
    private Material GLMaterial;
    [SerializeField]
    private GameObject fuelCanPrefab, particlePrefab;
    private List<GameObject> fuelCans;
    private List<ParticleSystem> particlerenderers;
    private static List<Vector3> points;
    [SerializeField]
    private float minDistance, maxDistance, minHeight, maxHeight;
    private static bool drawDebug;

    public static bool DrawDebug
    {
        get { return drawDebug; }
    }

    static public List<Vector3> Points
    {
        get { return points; }
    }

    private void Start()
    {
        points = new List<Vector3>();
        fuelCans = new List<GameObject>();
        particlerenderers = new List<ParticleSystem>();
        TerrainGenerator.reset += Reset;

        Reset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ToggleDebug();
        }
    }

    private void Reset()
    {
        if(points != null)
        {
            points.Clear();
        }

        foreach(GameObject fuelCan in fuelCans)
        {
            Destroy(fuelCan);
        }

        foreach(ParticleSystem particle in particlerenderers)
        {
            Destroy(particle.gameObject);
        }

        fuelCans.Clear();
        particlerenderers.Clear();

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

            fuelCans.Add(Instantiate(fuelCanPrefab, points[i], Quaternion.Euler(0, 0, 0), transform));
            particlerenderers.Add(Instantiate(particlePrefab, points[i], Quaternion.Euler(0, 0, 0), transform).GetComponent<ParticleSystem>());
        }

        for(int i = 0; i < points.Count; i++)
        {
            Vector3 point1 = points[i];
            Vector3 point2 = Vector3.zero;

            if(i == points.Count - 1)
            {
                point2 = points[0];
            }
            else
            {
                point2 = points[i + 1];
            }

            particlerenderers[i].transform.LookAt(point2);

            float distance = (point2 - point1).magnitude;

            particlerenderers[i].startLifetime = distance / 25f;
        }
    }

    public void ToggleDebug()
    {
        if (drawDebug)
        {
            drawDebug = false;
        }
        else
        {
            drawDebug = true;
        }
    }

    private void OnRenderObject()
    {
        if (drawDebug)
        {
            if (points.Count > 0)
            {
                GLMaterial.SetPass(0);

                GL.Color(Color.red);
                GL.Begin(GL.LINES);

                for (int i = 0; i < points.Count; i++)
                {
                    if (i == 0)
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ResetScene();

public class TerrainGenerator : MonoBehaviour {
    
    private TerrainData terrainData;
    private float[,] heightMap;
    private float[,,] splatMap;
    [SerializeField, Range(0, 1)]
    private float step;
    [SerializeField]
    private float pathWidth;
    private static float terrainWidth = 250;
    public static ResetScene reset;

    public static float TerrainWidth
    {
        get { return terrainWidth; }
    }

	// Use this for initialization
	void Start () {
        terrainData = GetComponent<Terrain>().terrainData;

        Reset();
	}

    private void Reset()
    {
        heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
        SetHeights();
        SetPath();
        terrainData.SetHeights(0, 0, heightMap);
        SetSplatMap();
        terrainData.SetAlphamaps(0, 0, splatMap);

        if(reset != null)
        {
            reset();
        }
    }

    private void SetHeights()
    {
        Vector2 seed = new Vector2(Random.value, Random.value);

        for(int i = 0; i < heightMap.GetLength(0); i++)
        {
            for(int j = 0; j < heightMap.GetLength(1); j++)
            {
                heightMap[i, j] = Mathf.PerlinNoise(seed.x +  j * step, seed.y + i * step);
            }
        }
    }

    private void SetSplatMap()
    {
        splatMap = new float[terrainData.alphamapHeight, terrainData.alphamapWidth, 3];

        for(int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for(int x = 0; x < terrainData.alphamapWidth; x++)
            {
                //Get the current coordinate as a fraction of the alphaMap
                Vector2 normalizedPoint = new Vector2((float)x / terrainData.alphamapWidth, (float)y / terrainData.alphamapHeight);
                
                float angle = terrainData.GetSteepness(normalizedPoint.x, normalizedPoint.y);

                if (angle < 0.25f)
                {
                    splatMap[y, x, 0] = 0f;
                    splatMap[y, x, 1] = 1f;
                    splatMap[y, x, 2] = 0f;
                }
                else if (angle < 50f)
                {
                    if(angle < 15)
                    {
                        splatMap[y, x, 0] = 0.5f + ((angle - 0.25f) / 25f);
                        splatMap[y, x, 1] = 1 - (0.5f + ((angle - 0.25f) / 25f));
                        splatMap[y, x, 2] = 0f;
                    }
                    else
                    {
                        splatMap[y, x, 0] = 1f;
                        splatMap[y, x, 1] = 0f;
                        splatMap[y, x, 2] = 0f;
                    }
                }
                else
                {
                    splatMap[y, x, 0] = 0f;
                    splatMap[y, x, 1] = 0f;
                    splatMap[y, x, 2] = 1f;
                }
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3((((float)x / heightMap.GetLength(1)) * terrainWidth) + transform.position.x, heightMap[y,x] * 20,(((float)y / heightMap.GetLength(0)) * terrainWidth) + transform.position.z);
    }

    private void SetPath()
    {
        //Circle of radius 100
        for(int y = 0; y < heightMap.GetLength(0); y++)
        {
            for(int x = 0; x < heightMap.GetLength(1); x++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);

                float radius = Mathf.Sqrt(Mathf.Pow(worldPosition.x, 2) + Mathf.Pow(worldPosition.z, 2));

                if(Mathf.Abs(radius - 100) < pathWidth / 2)
                {
                    heightMap[y, x] = 0.5f;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Reset();
        }
    }
}

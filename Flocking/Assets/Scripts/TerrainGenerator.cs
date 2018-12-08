using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    
    private TerrainData terrainData;
    private float[,] heightMap;
    [SerializeField, Range(0, 1)]
    private float step;
    [SerializeField]
    private float pathWidth;
    private float terrainWidth = 250;

	// Use this for initialization
	void Start () {
        terrainData = GetComponent<Terrain>().terrainData;
        heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
        SetHeights();
        SetPath();
        terrainData.SetHeights(0, 0, heightMap);
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
}

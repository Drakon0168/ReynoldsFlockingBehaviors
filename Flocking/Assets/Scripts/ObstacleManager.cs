using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    private static List<Obstacle> obstacles;
    [SerializeField]
    private GameObject obstaclePrefab;
    [SerializeField]
    private int obstacleCount;

    public static List<Obstacle> Obstacles
    {
        get
        {
            if(obstacles == null)
            {
                obstacles = new List<Obstacle>();
            }

            return obstacles;
        }
        set { obstacles = value; }
    }

    private void Start()
    {
        TerrainGenerator.reset += Reset;

        Reset();
    }

    public static List<Obstacle> GetNearbyObstacles(Vector2 position, float range)
    {
        List<Obstacle> nearbyObstacles = new List<Obstacle>();

        foreach(Obstacle obstacle in obstacles)
        {
            if((obstacle.Position - position).sqrMagnitude < range * range)
            {
                nearbyObstacles.Add(obstacle);
            }
        }

        return nearbyObstacles;
    }

    private void SpawnObstacles()
    {
        float angleOffset = (2 * Mathf.PI) / obstacleCount;

        for(int i = 0; i < obstacleCount; i++)
        {
            float offset = (Random.value * 20) - 10;
            Vector3 position = new Vector3(Mathf.Cos(angleOffset * i) * (100 + offset), TerrainGenerator.PathHeight, Mathf.Sin(angleOffset * i) * (100 + offset));

            Instantiate(obstaclePrefab, position, Quaternion.Euler(0, 0, 0), transform);
        }
    }

    private void Reset()
    {
        foreach(Obstacle obstacle in Obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        obstacles.Clear();

        SpawnObstacles();
    }
}

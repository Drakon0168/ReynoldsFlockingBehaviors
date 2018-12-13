using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    private static List<Flocker> flockers;
    private static Vector3 centerPosition, averageDirection, seekPoint;
    [SerializeField]
    private int chariotCount;
    [SerializeField]
    private GameObject chariotPrefab;

    /// <summary>
    /// A list of all of the flockers in the scene
    /// </summary>
    public static List<Flocker> Flockers
    {
        get {
            if(flockers == null)
            {
                flockers = new List<Flocker>();
            }

            return flockers;
        }
        set { flockers = value; }
    }

    public static Vector3 FlockAlignment
    {
        get { return averageDirection; }
    }

    public static Vector2 CenterPosition
    {
        get { return centerPosition; }
    }

    public static Vector3 SeekPoint
    {
        get { return seekPoint; }
    }

    private void Start()
    {
        for(int i = 0; i < chariotCount; i++)
        {
            Instantiate(chariotPrefab, new Vector3(Random.value * TerrainGenerator.TerrainWidth - (TerrainGenerator.TerrainWidth / 2), 10, Random.value * TerrainGenerator.TerrainWidth - (TerrainGenerator.TerrainWidth / 2)), Quaternion.Euler(0,0,0), this.transform);
        }
    }

    void Update()
    {
        centerPosition = Vector3.zero;
        averageDirection = Vector3.zero;

        foreach(Flocker flocker in flockers)
        {
            centerPosition += flocker.transform.position;
            averageDirection += flocker.Velocity.normalized;
        }

        centerPosition /= flockers.Count;

        float angle = Mathf.Atan2(centerPosition.x, centerPosition.z) - (Mathf.PI / 5 + Mathf.PI / 2);

        seekPoint = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle) * -1) * 120;
    }
}

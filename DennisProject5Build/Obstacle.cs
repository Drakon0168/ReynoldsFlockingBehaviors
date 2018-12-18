using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Obstacle : MonoBehaviour {

    private new CapsuleCollider collider;

    public float Radius
    {
        get { return collider.radius * transform.lossyScale.x; }
    }

    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.z); }
    }
    
	void Start () {
        collider = GetComponent<CapsuleCollider>();
        ObstacleManager.Obstacles.Add(this);
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius * transform.lossyScale.x);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    [SerializeField, Tooltip("The speed in degrees t change the angle around the path.")]
    private float speed;
    private Vector3 position, velocity, acceleration;
    private float angle;

    void Update()
    {
        angle += speed * Time.deltaTime;

        position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * 100;
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(position.z * -1, position.x) * Mathf.Rad2Deg, 0);
        Debug.DrawRay(transform.position, transform.forward * 5, Color.yellow);
    }
}

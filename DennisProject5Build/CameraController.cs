using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Transform target, targetLook;
    [SerializeField]
    private Vector3 targetOffset;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool smoothRotation;
    [SerializeField]
    private float lookSmooth;
    private Vector3 targetPosition;
    private Vector3 velocity;

    private void Start()
    {
        if(targetLook == null)
        {
            targetLook = target;
        }
    }

    // Update is called once per frame
    void Update () {
        targetPosition = target.position + targetOffset;

        Vector3 targetVelocity = targetPosition - transform.position;
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, speed);

        velocity += (targetVelocity - velocity) * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        if (smoothRotation)
        {
            transform.forward += (targetLook.position - transform.position) * lookSmooth * Time.deltaTime;
        }
        else
        {
            transform.LookAt(targetLook);
        }
        
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : MonoBehaviour {

    [SerializeField]
    private float height;
    [SerializeField]
    private new Transform transform;

    void Update()
    {
        StayOnTerrain();
    }

    private void StayOnTerrain()
    {
        float height = this.height;
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * 20, Vector3.down);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Terrain")
            {
                height += hit.point.y;
                break;
            }
        }

        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}

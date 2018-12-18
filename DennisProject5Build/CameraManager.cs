using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes{
    Overhead,
    FlockAverage,
    FlockLead,
    PathFollower,
    RaceView
}

public class CameraManager : MonoBehaviour {

    [SerializeField]
    private Camera[] cameras;

    private void Start()
    {
        SwitchCameras(CameraTypes.Overhead);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCameras(CameraTypes.Overhead);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCameras(CameraTypes.FlockAverage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCameras(CameraTypes.FlockLead);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchCameras(CameraTypes.PathFollower);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchCameras(CameraTypes.RaceView);
        }
    }

    public void SwitchCameras(CameraTypes cameraType)
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = (i == (int)cameraType);
        }
    }
}

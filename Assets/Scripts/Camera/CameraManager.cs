using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private Dictionary<string, CinemachineVirtualCamera> cameras =
        new Dictionary<string, CinemachineVirtualCamera>();

    public CinemachineVirtualCamera focusedCamera;
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddCamera(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera.name, camera);
    }

    public void RemoveCamera(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera.name);
    }

    public void SetFocusCamera(string cameraId)
    {
        Debug.Log("Set focus camera");
        foreach (var c in cameras)
        {
            CinemachineVirtualCamera cam = c.Value;
            if (c.Key == cameraId)
            {
                cam.Priority = 10;
                focusedCamera = cam;
                Debug.Log($"Set focussed camera to {cam.name}");
            }
            else
            {
                cam.Priority = 0;
            }
        }

        
    }

    public bool IsFocusCamera(string cameraId)
    {
        return focusedCamera != null && focusedCamera.name == cameraId;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;
    public int initialCamera = 0;
    public Transform initialTarget;
    
    public bool fixCameraToTarget = true;
    public KeyCode fixCameraKey = KeyCode.Y;

    int current = 0;

    Dictionary<Camera, Vector3> cameraOffset;
    Transform target;

    Vector3 lastOffset;

    void Awake()
    {
        cameraOffset = new Dictionary<Camera, Vector3>();
        
        SetTarget(initialTarget);
        SetCamera(initialCamera);
    }

    public void NextCamera()
    {
        int next = (current + 1) % cameras.Length;
        // current = next;
        
        SetCamera(next);
    }

    void SetCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(index == i);
        }

        current = index;
    }

    void SetTarget(Transform targetTransform)
    {
        foreach (Camera cam in cameras)
        {
            cameraOffset[cam] = cam.transform.position - targetTransform.position;
        }

        target = targetTransform;
    }

    void Update()
    {
        if (Input.GetKeyDown(fixCameraKey))
        {
            fixCameraToTarget = !fixCameraToTarget;
        }

        Vector3 offset;
        if (fixCameraToTarget)
        {
            offset = target.position;
            lastOffset = offset;
        }
        else
        {
            offset = lastOffset;
        }

        cameras[current].transform.position = cameraOffset[cameras[current]] + offset;
    }
}

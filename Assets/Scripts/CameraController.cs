using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;
    public int initialCamera = 0;
    public Transform initialTarget;

    int current = 0;

    Dictionary<Camera, Vector3> cameraOffset;
    Transform target;

    void Awake()
    {
        cameraOffset = new Dictionary<Camera, Vector3>();
        
        SetCamera(initialCamera);
        SetTarget(initialTarget);
    }

    public void NextCamera()
    {
        int next = (current + 1) % cameras.Length;
        current = next;
        
        SetCamera(next);
    }

    void SetCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(index == i);
        }
    }

    void SetTarget(Transform transform)
    {
        foreach (Camera cam in cameras)
        {
            cameraOffset[cam] = cam.transform.position - transform.position;
        }

        target = transform;
    }

    void Update()
    {
        cameras[current].transform.position = cameraOffset[cameras[current]] + target.position;
    }
}

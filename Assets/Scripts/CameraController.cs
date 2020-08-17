using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public HeroInput input;
    
    public Camera[] cameras;
    public int initialCamera = 0;
    public Transform initialTarget;
    
    public bool fixCameraToTarget = true;
    public KeyCode fixCameraKey = KeyCode.Y;

    public Vector3 threshold;
    public float multiplier;

    int current = 0;

    public int CurrentCameraIndex
    {
        get
        {
            return current;
        }
    }

    Dictionary<Camera, Vector3> cameraOffset;
    Transform target;

    Vector3 lastOffset;

    void Awake()
    {
        cameraOffset = new Dictionary<Camera, Vector3>();
        
        SetTarget(initialTarget);
        SetCamera(initialCamera);
        
        input.AddBindings(fixCameraKey, ETriggerType.DOWN, OnToggleFixCamera);
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
        Vector3 offset;
        if (fixCameraToTarget)
        {
            offset = target.position;
            lastOffset = offset;
        }
        else
        {
            offset = lastOffset ;
        }

        cameras[current].transform.position = cameraOffset[cameras[current]] + offset;
    }

    void OnToggleFixCamera()
    {
        fixCameraToTarget = !fixCameraToTarget;
    }

    Vector3 GetMouseInput()
    {
        var mouse = Input.mousePosition;
    
        Vector3 direction = new Vector3();
        if (mouse.x < threshold.x)
        {
            // Move Left
            direction += Vector3.left;
        }
        else if (mouse.x > Screen.width - threshold.x)
        {
            // Move Right
            direction += Vector3.right;
        }
    
        if (mouse.y < threshold.y)
        {
            // Move Down
            direction += Vector3.back;
        }
        else if (mouse.y > Screen.height - threshold.y)
        {
            // Move Up
            direction += Vector3.forward;
        }
    
        return direction;
    }
}

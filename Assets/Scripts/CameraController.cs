using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    static CameraController _instance;
    public static CameraController Instance {
        get
        {
            return _instance;
        }
    }
    
    [Header("카메라 기본")]
    public Camera[] cameras;
    public int initialCamera = 0;
    
    [Header("카메라 고정")]
    public bool fixCameraToTarget;
    bool forceFixCameraToTarget;
    public KeyCode fixCameraKey = KeyCode.Y;
    
    [HideInInspector]
    public UnityEvent onFixCameraToggle;

    [Header("카메라 마우스 이동 (WIP)")]
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
        _instance = this;
        cameraOffset = new Dictionary<Camera, Vector3>();
        
        SetTarget(ObjectManager.GetPlayer().transform);
        SetCamera(initialCamera);
        
        InputHandler.AddBindings(fixCameraKey, ETriggerType.DOWN, OnToggleFixCamera);
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
        if (forceFixCameraToTarget || fixCameraToTarget)
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
        if (fixCameraToTarget)
        {
            UnfocusHero();
        }
        else
        {
            FocusHero();
        }
        
        onFixCameraToggle.Invoke();
    }

    public void FocusHero()
    {
        fixCameraToTarget = true;
    }

    public void UnfocusHero()
    {
        fixCameraToTarget = false;
    }

    public static  void ForceFocusHero()
    {
        _instance.forceFixCameraToTarget = true;
    }

    public static  void UnforceFocusHero()
    {
        _instance.forceFixCameraToTarget = false;
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

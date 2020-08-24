using UnityEngine;
using UnityEngine.Events;

public class CursorUtility : MonoBehaviour
{
    public LayerMask layer;

    [HideInInspector] public UnityEvent<Vector3> onRaycastSucess = new UnityEvent<Vector3>();
    
    // public bool debugger;
    // public GameObject debugSphere;
    
    static CursorUtility _instance;
    public static CursorUtility Instance {
        get { return _instance; }
    }

    Vector3 hitPoint;
    Collider hitCollider; 

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layer))
            {
                hitPoint = hit.point;
                hitCollider = hit.collider;
                
                onRaycastSucess.Invoke(hit.point);

                // if (debugger)
                // {
                //     debugSphere.transform.position = hit.point;
                // }
            }
            else
            {
                hitCollider = null;
            }
        }

    }

    // void FixedUpdate()
    // {
    //     if (debugger)
    //     {
    //         if (!debugSphere.gameObject.activeSelf)
    //             debugSphere.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         if (debugSphere.gameObject.activeSelf)
    //             debugSphere.gameObject.SetActive(false);
    //     }
    // }

    public static Vector3 GetMousePosition()
    {
        return _instance.hitPoint;
    }

    public static Vector3 GetMouseScreenPoition()
    {
        return Input.mousePosition;
    }

    public static void SetLayer(int layer)
    {
        _instance.layer = layer;
    }

    public static Collider GetUnderMouseObject()
    {
        return _instance.hitCollider;
    }

    // public static void SetDebugger(bool isOn)
    // {
    //     _instance.debugger = isOn;
    // }
    //
    // public static bool GetDebugger()
    // {
    //     return _instance.debugger;
    // }
}

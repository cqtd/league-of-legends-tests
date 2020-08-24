using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected Rigidbody rb;
    
    
    protected Vector3 destination;
    public Vector3 Destination { get { return destination; } }
    
    public abstract void UpdateDestination(Vector3 pos);
    public abstract void HoldPosition();
    
    
    protected Vector3 CurrentPosition()
    {
        return new Vector3(rb.position.x, 0, rb.position.z);
    }
}
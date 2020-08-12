using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    public HeroAnimController animController;
    public Rigidbody rb;

    public Vector3 forwardVector;
    public float speed = 10.0f;

    bool isMoving;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            rb.velocity = forwardVector * speed * Time.deltaTime; 
            // rb.AddForce();
        }
    }

    public void Move()
    {
        animController.isMoving = true;
        isMoving = true;
    }

    public void Idle()
    {
        animController.isMoving = false;
        isMoving = false;
    }
    
    
}

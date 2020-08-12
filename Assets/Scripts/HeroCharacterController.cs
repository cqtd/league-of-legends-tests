using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    public HeroAnimController animController;
    public Rigidbody rb;

    public Vector3 forwardVector;
    public float speed = 10.0f;
    public float addForceParameter = 1.0f;
    public float velocityParameter = 1.0f;

    bool isMoving;
    public bool addForce;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (addForce)
            {
                rb.AddForce(forwardVector * speed * Time.deltaTime * addForceParameter);
            }
            else
            {
                rb.velocity = forwardVector * speed * velocityParameter; 
            }
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

using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    public HeroAnimController animController;
    public Rigidbody rb;

    public Vector3 forwardVector;
    public float speed = 10.0f;
    public float addForceParameter = 1.0f;
    public float velocityParameter = 1.0f;
    public float rotationParameter = 10.0f;
    
    public bool addForce;

    public float holdRadius = 0.2f;
    public float threshold = 1f;

    public bool debugger;
    public GameObject destinationSphere;
    public float stopParameter = 0.1f;
    
    bool isArrived = true;
    bool isMoving;
    bool mouseDown = false;
    
    Vector3 destination;

    public KeyCode stopKey = KeyCode.S;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(stopKey))
        {
            destination = (destination - CurrentPosition()).normalized * stopParameter + CurrentPosition();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            mouseDown = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseDown = false;
        }
        
        if (mouseDown)
        {
            Idle();
            
            Vector3 dest = CursorUtility.GetMousePosition();
            dest.y = 0;

            // 새로 정해진 목적지가 유효한 경우
            if (Vector3.Distance(CurrentPosition(), dest) > holdRadius)
            {
                destination = dest;

                isArrived = false;
                animController.isMoving = true;
                
            }
        }

        var gap = destination - CurrentPosition();

        // 도착 했나?
        var sqrt = Vector3.SqrMagnitude(gap);
        
        // Vector3 dest2 = CursorUtility.GetMousePosition();
        // if(Vector3.Distance(CurrentPosition(), dest2) > holdRadius)
        if (sqrt < threshold)
        {
            isArrived = true;
            animController.isMoving = false;
        }

        // 도착 안 했다
        if (!isArrived)
        {
            var direction= gap;
            direction.Normalize();

            rb.velocity = direction * speed * velocityParameter;
        }

        // 캐릭터 로테이션
        if (gap != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(gap);
            rb.transform.rotation =
                Quaternion.Slerp(rb.transform.rotation, targetAngle, Time.deltaTime * rotationParameter);
        }

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

        if (debugger)
        {
            destinationSphere.transform.position = destination;
        }
    }

    void FixedUpdate()
    {
        if (debugger)
        {
            if (!destinationSphere.gameObject.activeSelf)
                destinationSphere.gameObject.SetActive(true);
        }
        else
        {
            if (destinationSphere.gameObject.activeSelf)
                destinationSphere.gameObject.SetActive(false);
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

    bool IsArrived()
    {
        var dest = destination;
        var cur = rb.position;
        
        dest.y = 0;
        cur.y = 0;

        var result = Vector3.SqrMagnitude(dest - cur);
        
        Debug.Log(result);
        
        return result < threshold;
    }

    Vector3 CurrentPosition()
    {
        return new Vector3(rb.position.x, 0, rb.position.z);
    }
    
}

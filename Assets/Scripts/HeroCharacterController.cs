using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    [Header("Component")]
    public HeroAnimController animController;
    public Rigidbody rb;
    
    [Header("Bindings")]
    public KeyCode stopKey = KeyCode.S;

    [Header("Parameter")]
    [Range(300, 500)] public float speed = 350.0f;
    [Range(0.001f, 1)] public float velocityParameter = 0.008f;
    [Range(5, 20)] public float rotationParameter = 10.0f;
    [Range(-2, 2)] public float stopParameter = 1f;

    [Space]
    public float holdRadius = 0.2f;
    public float threshold = 1f;

    [Header("Particle")]
    public bool spawnDestination;
    public GameObject destDecalprefab;
    public float decalInitialScale = 0.08f;
    public float decalDuration = 0.2f;
    
    [Header("Debug")]
    public bool debugger;
    public GameObject destinationSphere;
    public float debugOffset = 0.01f;
    
    bool isArrived = true;
    bool isMoving;
    bool mouseDown = false;
    
    Vector3 destination;
    
    void Update()
    {
        if (Input.GetKeyDown(stopKey))
        {
            destination = (destination - CurrentPosition()).normalized * stopParameter + CurrentPosition();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            mouseDown = true;
            
            if (spawnDestination)
            {
                StartCoroutine(Spawn());
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseDown = false;
        }
        
        if (mouseDown)
        {
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

        if (debugger)
        {
            destinationSphere.transform.position = destination + Vector3.up * debugOffset;
        }
    }

    //@todo: pooling system
    IEnumerator Spawn()
    {
        yield return null;
        yield return null;
        
        var decal = Instantiate(destDecalprefab, transform);
        decal.transform.position = destination + Vector3.up * debugOffset;
        decal.transform.rotation = Quaternion.identity;
        decal.transform.localScale = Vector3.one * decalInitialScale;

        var tween = decal.transform.DOScale(0.0f, decalDuration);
        tween.onComplete += () =>
        {
            Destroy(decal.gameObject);
        };
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

    Vector3 CurrentPosition()
    {
        return new Vector3(rb.position.x, 0, rb.position.z);
    }
}

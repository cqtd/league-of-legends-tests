using System.Collections;
using UnityEngine;

public class HeroController : ControllerBase
{
    [Header("Component")]
    public HeroAnimMachine heroAnimMachine;
    
    [Header("Bindings")]
    public KeyCode stopKey = KeyCode.S;

    [Header("Parameter")]
    [Range(300, 500)] public float speed = 350.0f;
    [Range(0.001f, 1)] public float velocityParameter = 0.008f;
    [Range(5, 20)] public float rotationParameter = 10.0f;
    [Range(-2, 2)] public float stopParameter = 1f;

    [Space] 
    [Range(0.01f, 0.5f)] public float holdRadius = 0.2f;
    public float threshold = .5f;

    [Header("Particle")] 
    public bool spawnDestination = true;
    public float zFightOffset = 0.01f;

    bool isArrived = true;
    bool mouseDown;

    void Start()
    { 
        InputHandler.AddBindings(EMouseButton.Right, ETriggerType.DOWN, OnTryUpdateDestination);
    }

    void Update()
    {
        var gap = destination - CurrentPosition();

        // 도착 했나?
        var sqrt = Vector3.SqrMagnitude(gap);
        
        if (sqrt < threshold)
        {
            isArrived = true;
            heroAnimMachine.isMoving = false;
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
    }

    void OnTryUpdateDestination()
    {
        if (spawnDestination)
        {
            StartCoroutine(Spawn());
        }
    }
    
    public override void UpdateDestination(Vector3 pos)
    {
        Vector3 dest = pos;
        dest.y = 0;

        // 새로 정해진 목적지가 유효한 경우
        if (Vector3.Distance(CurrentPosition(), dest) > holdRadius)
        {
            destination = dest;

            isArrived = false;
            heroAnimMachine.isMoving = true;
        }
    }

    public override void HoldPosition()
    {
        destination = (destination - CurrentPosition()).normalized * stopParameter + CurrentPosition();
    }


    IEnumerator Spawn()
    {
        yield return null;
        CursorEffect.Spawn(destination + Vector3.up * zFightOffset , null);
    }
}
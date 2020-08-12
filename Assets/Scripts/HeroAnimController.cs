using System.Collections;
using UnityEngine;

public class HeroAnimController : MonoBehaviour
{
    public Animator animator;

    public bool idle2;
    public bool idle3;
    public bool isMoving;

    bool idle2Flag;

    void Update()
    {
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("Idle2", idle2);
        animator.SetBool("Idle3", idle3);
    }
}

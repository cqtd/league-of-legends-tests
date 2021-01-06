using System;
using System.Linq;

public class HeroAnimMachine : AnimMachineBase
{
    public bool collectParameters;
    public string[] animationParamters = new string[0];

    private AnimationParameterLibrary library;
    
    [NonSerialized] public bool idle2;
    [NonSerialized] public bool idle3;
    [NonSerialized] public bool isMoving;

    private void Awake()
    {
        if (collectParameters)
        {
            library = new AnimationParameterLibrary(animator.parameters.Select(e => e.name).ToArray());
        }
        else
        {
            library = new AnimationParameterLibrary(animationParamters);
        }
    }

    private void Update()
    {
        animator.SetBool(library.GetHash("IsMoving"), isMoving);
        animator.SetBool(library.GetHash("Idle2"), idle2);
        animator.SetBool(library.GetHash("Idle3"), idle3);
    }
}
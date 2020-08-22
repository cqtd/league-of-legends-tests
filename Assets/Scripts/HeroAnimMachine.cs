using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroAnimMachine : AnimMachineBase
{
    public bool collectParameters;
    public string[] animationParamters = new string[0];
    
    AnimationParameterLibrary library;
    
    [NonSerialized] public bool idle2;
    [NonSerialized] public bool idle3;
    [NonSerialized] public bool isMoving;

    void Awake()
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

    void Update()
    {
        animator.SetBool(library.GetHash("IsMoving"), isMoving);
        animator.SetBool(library.GetHash("Idle2"), idle2);
        animator.SetBool(library.GetHash("Idle3"), idle3);
    }
}

[Serializable]
public class AnimationParameterLibrary
{
    readonly Dictionary<string, int> hashes;
    
    public AnimationParameterLibrary(IEnumerable<string> names)
    {
        hashes = new Dictionary<string, int>();

        foreach (string name in names)
        {
            hashes[name] = Animator.StringToHash(name);
        }
    }

    public int GetHash(string name)
    {
        return !hashes.TryGetValue(name, out int hash) ? 0 : hash;
    }
}
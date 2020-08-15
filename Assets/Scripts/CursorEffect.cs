using System;
using DG.Tweening;
using UnityEngine;

public class CursorEffect : MonoBehaviour
{
    public MeshRenderer[] renderers;
    public Material material;
    public float duration = 0.4f;
    
    Material instance;
    
    void Awake()
    {
        instance = new Material(material);
        foreach (MeshRenderer meshRenderer in renderers)
        {
            meshRenderer.sharedMaterial = instance;
        }

        instance.SetFloat("_Shift", -1.0f);
    }

    void Start()
    {
        var tweener = instance.DOFloat(2, "_Shift", duration);
        tweener.onComplete += () =>
        {
            Destroy(this.gameObject);
        };
    }
}
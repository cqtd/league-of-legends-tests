using System;
using DG.Tweening;
using UnityEngine;

public class CursorEffect : MonoBehaviour
{
    [Header("Arrows")]
    public MeshRenderer[] renderers;
    public Material material;
    public float duration = 0.4f;

    [Header("Ring")] 
    public GameObject ring;
    public float ringDuration = 0.4f;
    public float initialScale = 0.08f;
    
    Material instance;
    
    void Awake()
    {
        instance = new Material(material);
        foreach (MeshRenderer meshRenderer in renderers)
        {
            meshRenderer.sharedMaterial = instance;
        }

        instance.SetFloat("_Shift", -1.0f);
        
        ring.transform.localScale = Vector3.one * initialScale;
    }

    void Start()
    {
        var tweener = instance.DOFloat(2, "_Shift", duration);
        tweener.onComplete += () =>
        {
            Destroy(this.gameObject);
        };
        
        
        var tween = ring.transform.DOScale(0.0f, ringDuration);
        tween.onComplete += () =>
        {
            ring.gameObject.SetActive(false);
            
            // Destroy(ring.gameObject);
        };
    }
}
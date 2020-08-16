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
    Tweener arrowTweener;
    Tweener ringTweener;
    
    static readonly int shift = Shader.PropertyToID("_Shift");

    static CursorEffect last;

    void Awake()
    {
        if (last != null)
        {
            Destroy(last.gameObject);
        }
        
        last = this;
        
        instance = new Material(material);
        foreach (MeshRenderer meshRenderer in renderers)
        {
            meshRenderer.sharedMaterial = instance;
        }

        instance.SetFloat(shift, -1.0f);
        
        ring.transform.localScale = Vector3.one * initialScale;
    }

    void Start()
    {
        arrowTweener = instance.DOFloat(2, shift, duration);
        arrowTweener.onComplete += OnArrowComplete;
        
        ringTweener = ring.transform.DOScale(0.0f, ringDuration);
        ringTweener.onComplete += OnRingComplete;
    }

    void OnArrowComplete()
    {
        Destroy(this.gameObject);
    }

    void OnRingComplete()
    {
        ring.gameObject.SetActive(false);
    }
}
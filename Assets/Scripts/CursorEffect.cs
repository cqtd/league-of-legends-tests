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
    
    Material instancedMaterial;
    Tweener arrowTweener;
    Tweener ringTweener;
    
    static readonly int shift = Shader.PropertyToID("_Shift");

    static CursorEffect _instance;

    void Awake()
    {
        instancedMaterial = new Material(material);
        foreach (MeshRenderer meshRenderer in renderers)
        {
            meshRenderer.sharedMaterial = instancedMaterial;
        }

        instancedMaterial.SetFloat(shift, -1.0f);
    }

    void OnArrowComplete()
    {
        
    }

    void OnRingComplete()
    {
        
    }

    public static void Spawn(Vector3 pos, Transform parent)
    {
        if (_instance != null)
        {
            _instance.StopAnimation();
        }
        else
        {
            _instance = Instantiate(Resources.Load<CursorEffect>("CursorEffect"), parent);
            _instance.transform.position = pos;
            _instance.transform.rotation = Quaternion.identity;
            
            _instance.SetupAnimation();
        }
        
        _instance.StartAnimation(pos);
    }

    void SetupAnimation()
    {
        arrowTweener = instancedMaterial.DOFloat(2, shift, duration);
        arrowTweener.onComplete += OnArrowComplete;
        arrowTweener.SetAutoKill(false);
        
        ringTweener = ring.transform.DOScale(0.0f, ringDuration);
        ringTweener.onComplete += OnRingComplete;
        ringTweener.SetAutoKill(false);
    }

    void StartAnimation(Vector3 pos)
    {
        _instance.transform.position = pos;
        ring.transform.localScale = Vector3.one * initialScale;

        arrowTweener.Restart(false);
        ringTweener.Restart(false);
    }

    void StopAnimation()
    {
        arrowTweener.Pause();
        ringTweener.Pause();
    }
}
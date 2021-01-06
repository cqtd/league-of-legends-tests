using UnityEngine;

[CreateAssetMenu(menuName="Animator/Config")]
public class AnimatorConfig : ScriptableObject
{
    [SerializeField] private RuntimeAnimatorController m_controller = default;
    
    [SerializeField] private AnimationClip[] m_idleAnimations = default;
    [SerializeField] private AnimationClip m_walkAnimation = default;

    public void Bind(Animator animator)
    {
        animator.runtimeAnimatorController = m_controller;
    }
}
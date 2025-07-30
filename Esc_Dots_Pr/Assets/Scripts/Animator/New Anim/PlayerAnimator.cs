using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator Animator;

    private void Awake()
    {
        if (Animator == null)
            Animator = GetComponent<Animator>();
    }
    public void SetSpeed(float speed)
    {
        if (Animator != null)
        {
            Animator.SetFloat("Speed", speed);
        }
    }
}
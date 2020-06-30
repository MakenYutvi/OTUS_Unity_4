using UnityEngine;


public sealed class PlayerAnimation : MonoBehaviour
{
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int FireDisable = Animator.StringToHash("FireDisable");
    private static readonly int FireEnable = Animator.StringToHash("FireEnable");
    private static readonly int JumpEnable = Animator.StringToHash("JumpEnable");
    private static readonly int JumpDisable = Animator.StringToHash("JumpDisable");
    private static readonly int RotateLeftBegin = Animator.StringToHash("RotateLeftBegin");
    private static readonly int RotateLeftEnd = Animator.StringToHash("RotateLeftEnd");
    private static readonly int RotateRightBegin = Animator.StringToHash("RotateRightBegin");
    private static readonly int RotateRightEnd = Animator.StringToHash("RotateRightEnd");
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void SetMove(Vector3 movement)
    {
        Animator.SetFloat(Vertical, movement.z);
        Animator.SetFloat(Horizontal, movement.x);
    }

    public void OnFireEnable()
    {
        Animator.SetTrigger(FireEnable);
    }

    public void OnFireDisable()
    {
        Animator.SetTrigger(FireDisable);
    }

    public void OnJumpEnable()
    {
        Animator.SetTrigger(JumpEnable);
    }
    public void OnJumpDisable()
    {
        Animator.SetTrigger(JumpDisable);
    }

    public void OnRotateLeftBegin()
    {
        Animator.SetTrigger(RotateLeftBegin);
    }

    public void OnRotateLeftEnd()
    {
        Animator.SetTrigger(RotateLeftEnd);
    }

    public void OnRotateRightBegin()
    {
        Animator.SetTrigger(RotateRightBegin);
    }

    public void OnRotateRightEnd()
    {
        Animator.SetTrigger(RotateRightEnd);
    }


}

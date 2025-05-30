using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;

public class Hook : PlayerSkill, ISkill
{
    public Hook(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = false;
    }

    private readonly int _hook = Animator.StringToHash("Hook");

    public override void Fire()
    {
        
    }

    public void SetMoveTransform(Transform targetTransform)
    {
        if(targetTransform == null)
        {
            _animator.SetBool(_hook, false);
            EndSkill = true;
            return;
        }

        _animationEvent.StartCoroutine(HookMovement(targetTransform));
    }

    private IEnumerator HookMovement(Transform targetTrasnfom)
    {
        yield return null;
    }

    public override void InitializeSkill(Transform firePosition, int animatorTriggerCode)
    {
        base.InitializeSkill(firePosition, animatorTriggerCode);
    }

    public override void Reloading()
    {

    }

    public override void OnUpdateSkill() { }
    
    public override void Execute()
    {
        base.Execute();
        Fire();
    }
}

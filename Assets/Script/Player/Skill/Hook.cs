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

    public override void Reloading()
    {

    }

    public override void OnUpdateSkill() { }
    
    public override void Execute()
    {
        _animationEvent.SetReloadAction(Reloading);

        _animator.SetTrigger(_skill);
        _animator.SetInteger(_skillEquals, _skillInfo.AnimationCode);

        Fire();
    }

    public override void InitializeSkill(SkillInfo info)
    {
        _skillInfo = info;
    }
}

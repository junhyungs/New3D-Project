using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using System;

public class FireBall : PlayerSkill, ISkill
{
    public FireBall(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = true;
    }

    public override void Execute()
    {
        _animationEvent.SetReloadAction(Reloading);

        _animator.SetTrigger(_skill);
        _animator.SetInteger(_skillEquals, _skillInfo.AnimationCode);

        _rotate = true;
    }

    public override void Fire()
    {
        _rotate = false;

        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        var success = stateInfo.normalizedTime >= 0.9f;

        if (success)
        {
            Debug.Log("발사");
        }
        else
        {
            Debug.Log("스킬취소");
            _animationEvent.UnSetReloadAction();
        }

        Action action = success ? () => _animator.SetTrigger(_complete) :
            () => _animator.SetTrigger(_skillFail);

        action.Invoke();
        EndSkill = true;
    }

    public override void InitializeSkill(SkillInfo info)
    {
        _skillInfo = info;
    }

    public override void Reloading()
    {
        
    }

}

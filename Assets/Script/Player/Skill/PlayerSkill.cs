using EnumCollection;
using PlayerComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerSkill : ISkill
{
    public PlayerSkill(PlayerAnimationEvent animationEvent)
    {
        _animationEvent = animationEvent;
        
        var player = animationEvent.GetComponentInParent<Player>();
        _playerTransform = player.transform;

        var animator = _animationEvent.Animator;
        if(animator != null )
            _animator = animator;
    }

    protected PlayerAnimationEvent _animationEvent;
    protected Animator _animator;
    protected Transform _playerTransform;
    protected Transform _fireTransform;
    protected int _animationCode;
    protected readonly int _skill = Animator.StringToHash("Skill");
    protected readonly int _skillEquals = Animator.StringToHash("SkillEquals");
    protected readonly int _complete = Animator.StringToHash("Complete");
    protected readonly int _skillFail = Animator.StringToHash("SkillFail");

    public bool RequiresReload { get; set; }
    public bool EndSkill { get; set; }

    public virtual void InitializeSkill(Transform firePosition, int animatorTriggerCode)
    {
        _fireTransform = firePosition;
        _animationCode = animatorTriggerCode;
    }

    public virtual void Execute()
    {
        _animationEvent.SetReloadAction(Reloading);

        _animator.SetTrigger(_skill);
        _animator.SetInteger(_skillEquals, _animationCode);
    }

    public virtual void Reloading() { }
    public virtual void Fire()
    {
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

    public virtual void RemoveProjectile()
    {
        //TODO 자식으로 붙어있는 투사체 제거.
        _animationEvent.UnSetReloadAction();
        EndSkill = false;
    }
}

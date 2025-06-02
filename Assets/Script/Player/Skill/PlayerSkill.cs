using EnumCollection;
using PlayerComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : ISkill
{
    public PlayerSkill(PlayerAnimationEvent animationEvent)
    {
        _animationEvent = animationEvent;
        
        _player = animationEvent.GetComponentInParent<Player>();        
        _playerPlane = _player.GetComponent<PlayerPlane>();
        _rigidBody = _player.GetComponent<Rigidbody>();

        var animator = _animationEvent.Animator;
        if(animator != null )
            _animator = animator;
    }

    protected PlayerAnimationEvent _animationEvent;
    protected Player _player;
    protected Rigidbody _rigidBody;
    protected PlayerPlane _playerPlane;
    protected Animator _animator;
    protected SkillInfo _skillInfo;
    protected Action _fireAction;
    
    protected readonly int _skill = Animator.StringToHash("Skill");
    protected readonly int _skillEquals = Animator.StringToHash("SkillEquals");
    protected readonly int _complete = Animator.StringToHash("Complete");
    protected readonly int _skillFail = Animator.StringToHash("SkillFail");

    protected bool _rotate;

    public bool RequiresReload { get; set; }
    public bool EndSkill { get; set; }

    public abstract void InitializeSkill(SkillInfo info);
    public abstract void Execute();
    public abstract void Reloading();
    public abstract void Fire();
    public virtual void RemoveProjectile()
    {
        //TODO 자식으로 붙어있는 투사체 제거.
        _animationEvent.UnSetReloadAction();
        EndSkill = false;
    }

    public virtual void OnUpdateSkill()
    {
        if (!_rotate)
            return;

        Rotate();
    }

    private void Rotate()
    {
        Vector3 lookPos = new Vector3(_playerPlane.Point.x,
           _player.transform.position.y, _playerPlane.Point.z);

        Vector3 rotateDirection = (lookPos - _player.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(rotateDirection);

        _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation,
            lookRotation, 700f * Time.deltaTime);
    }

    protected void IsComplete(bool success)
    {
        Action action = success ? () => _animator.SetTrigger(_complete) :
            () => _animator.SetTrigger(_skillFail);

        action.Invoke();
        EndSkill = true;
    }

    protected void MakeProjectile(ObjectKey key, int count = 1)
    {
        ObjectPool.Instance.CreatePool(key, count);
    }
}

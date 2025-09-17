using EnumCollection;
using GameData;
using PlayerComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : ISkill
{
    public PlayerSkill(PlayerAnimationEvent animationEvent, PlayerSkillSystem skillSystem)
    {
        _animationEvent = animationEvent;
        
        _player = animationEvent.GetComponentInParent<Player>();        
        _playerPlane = _player.GetComponent<PlayerPlane>();
        _rigidBody = _player.GetComponent<Rigidbody>();
        _skillSystem = skillSystem;

        var animator = _animationEvent.Animator;
        if(animator != null )
            _animator = animator;
    }

    protected PlayerAnimationEvent _animationEvent;
    protected PlayerSkillSystem _skillSystem;
    protected SkillData _data;
    protected Player _player;
    protected Rigidbody _rigidBody;
    protected PlayerPlane _playerPlane;
    protected Animator _animator;
    protected SkillInfo _skillInfo;
    protected Action _fireAction;
    protected string _address;
    
    protected readonly int _skill = Animator.StringToHash("Skill");
    protected readonly int _skillEquals = Animator.StringToHash("SkillEquals");
    protected readonly int _complete = Animator.StringToHash("Complete");
    protected readonly int _skillFail = Animator.StringToHash("SkillFail");

    protected bool _rotate;

    public bool RequiresReload { get; set; }
    public bool EndSkill { get; set; }

    public abstract void InitializeSkill(SkillInfo info, SkillData data);
    public abstract void Execute();
    public abstract void Reloading();
    public abstract void Fire();
    public virtual void RemoveProjectile()
    {
        if(_skillInfo.FireTransform.childCount > 0)
        {
            var childObject = _skillInfo.FireTransform.GetChild(0).gameObject;
            childObject.transform.parent = null;
            PlayerProjectilePool.Instance.EnqueueGameObject(_address, childObject);
        }

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

    protected bool TryUse()
    {
        var useCost = _data.Cost;
        if (_skillSystem.Cost >= useCost)
        {
            _skillSystem.Cost -= useCost;
            return true;
        }

        return false;
    }

    protected void IsComplete(bool success)
    {
        Action action = success ? () => _animator.SetTrigger(_complete) :
            () => _animator.SetTrigger(_skillFail);

        action.Invoke();
        EndSkill = true;
    }

    protected void MakeProjectile(string address, int count = 1)
    {
        PlayerProjectilePool.Instance.CreatePool(address, count);
    }

    public int GetCost()
    {
        if(_data == null)
            return 0;

        return _data.Cost;
    }
}

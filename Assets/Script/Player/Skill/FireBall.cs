using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using System;
using EnumCollection;
using GameData;

public class FireBall : PlayerSkill, ISkill
{
    public FireBall(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = true;

        _objectKey = ObjectKey.PlayerFireBallPrefab;
        MakeProjectile(_objectKey);
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
            _fireAction?.Invoke();
        }
        else
        {
            _animationEvent.UnSetReloadAction();
        }

        IsComplete(success);
    }

    public override void InitializeSkill(SkillInfo info, PlayerSkillData data)
    {
        _data = data;
        _skillInfo = info;
    }

    public override void Reloading()
    {
        var fireBallObject = ObjectPool.Instance.DequeueGameObject(_objectKey);
        fireBallObject.transform.SetParent(_skillInfo.FireTransform);
        fireBallObject.transform.localPosition = Vector3.zero;
        fireBallObject.transform.localRotation = Quaternion.identity;

        var fireBallComponent = fireBallObject.GetComponent<FireBallObject>();
        if(fireBallComponent != null)
        {
            fireBallComponent.SetData(_data.FlightTime, _data.ProjectileSpeed,
                _data.ProjectileDamage);

            Action action = null;
            action = () =>
            {
                fireBallObject.transform.parent = null;
                fireBallComponent.Fire();
                _fireAction -= action;
            };

            _fireAction += action;
        }
        else
        {
            IsComplete(false);
        }
    }

}

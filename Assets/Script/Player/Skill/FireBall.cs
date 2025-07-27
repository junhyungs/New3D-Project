using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using System;
using EnumCollection;
using GameData;

public class FireBall : PlayerSkill, ISkill
{
    public FireBall(PlayerAnimationEvent animationEvent, PlayerSkillSystem playerSkillSystem) : base(animationEvent, playerSkillSystem)
    {
        RequiresReload = true;

        _address = AddressablesKey.Prefab_PlayerFireball;
        MakeProjectile(_address);
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

    public override void InitializeSkill(SkillInfo info, SkillData data)
    {
        _skillInfo = info;
        _data = data;
    }

    public override void Reloading()
    {
        if (!TryUse())
            return;

        var fireBallObject = PlayerProjectilePool.Instance.DequeueGameObject(_address);
        fireBallObject.transform.SetParent(_skillInfo.FireTransform);
        fireBallObject.transform.localPosition = Vector3.zero;
        fireBallObject.transform.localRotation = Quaternion.identity;

        var fireBallComponent = fireBallObject.GetComponent<FireBallObject>();
        if(fireBallComponent != null)
        {
            fireBallComponent.SetData(_data.FlightTime, _data.Speed,
                _data.Damage);

            fireBallComponent.PlayParticleSystem(true);

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

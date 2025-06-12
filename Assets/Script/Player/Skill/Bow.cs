using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumCollection;
using GameData;

public class Bow : PlayerSkill, ISkill
{
    public Bow(PlayerAnimationEvent animationEvent, PlayerSkillSystem playerSkillSystem) : base(animationEvent, playerSkillSystem)
    {
        RequiresReload = true;

        _objectKey = ObjectKey.PlayerArrowPrefab;
        MakeProjectile(_objectKey);
    }

    public override void Execute()
    {
        _skillInfo.SkillItem.SetActive(true);

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

        _skillInfo.SkillItem.SetActive(false);
        IsComplete(success);
    }

    public override void InitializeSkill(SkillInfo info, PlayerSkillData data)
    {
        _data = data;
        _skillInfo = info;
    }

    public override void Reloading()
    {
        if (!TryUse())
            return;

        var arrowObject = ObjectPool.Instance.DequeueGameObject(ObjectKey.PlayerArrowPrefab);
        arrowObject.transform.SetParent(_skillInfo.FireTransform);
        arrowObject.transform.localPosition = Vector3.zero;
        arrowObject.transform.localRotation = Quaternion.identity;

        var arrowComponent = arrowObject.GetComponent<ArrowObject>();
        if(arrowComponent != null)
        {
            arrowComponent.SetData(_data.FlightTime,
                _data.ProjectileSpeed, _data.ProjectileDamage);

            Action action = null;
            action = () =>
            {
                arrowObject.transform.parent = null;
                arrowComponent.Fire();
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

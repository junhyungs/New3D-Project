using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumCollection;
using GameData;

public class Bomb : PlayerSkill, ISkill
{
    public Bomb(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = true;

        _objectKey = ObjectKey.PlayerBombPrefab;
        MakeProjectile(_objectKey);
    }

    private const string _arrowBombEnd = "Arrow_bomb_end";
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

        Action action = success ? () => _animator.SetTrigger(_complete) :
            () => _animator.SetTrigger(_skillFail);

        if (success)
        {
            _fireAction?.Invoke();
            _animationEvent.StartCoroutine(DelayCoroutine(action));
        }
        else
        {
            action.Invoke();
            EndSkill = true;

            _animationEvent.UnSetReloadAction();
        }
    }

    private IEnumerator DelayCoroutine(Action action)
    {
        action.Invoke();

        yield return new WaitUntil(() =>
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(_arrowBombEnd);
        });

        while(_animator.IsInTransition(0))
            yield return null;

        EndSkill = true;
    }

    public override void Reloading()
    {
        var bombObject = ObjectPool.Instance.DequeueGameObject(_objectKey);
        bombObject.transform.SetParent(_skillInfo.FireTransform);
        bombObject.transform.localPosition = Vector3.zero;
        bombObject.transform.localRotation = Quaternion.identity;

        var bombComponent = bombObject.GetComponent<BombObject>();
        if(bombComponent != null)
        {
            bombComponent.SetData(_data.FlightTime, _data.ProjectileSpeed,
                _data.ProjectileDamage);

            Action action = null;
            action = () =>
            {
                bombObject.transform.parent = null;
                bombComponent.Fire();
                _fireAction -= action;
            };

            _fireAction += action;
        }
        else
        {
            _animator.SetTrigger(_skillFail);
            EndSkill = true;
        }
    }

    public override void InitializeSkill(SkillInfo info, PlayerSkillData data)
    {
        _data = data;
        _skillInfo = info;
    }
}

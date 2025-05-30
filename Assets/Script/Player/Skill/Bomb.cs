using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : PlayerSkill, ISkill
{
    public Bomb(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = true;
    }

    private const string _arrowBombEnd = "Arrow_bomb_end";

    public override void Fire()
    {
        _rotate = false;

        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        var success = stateInfo.normalizedTime >= 0.9f;

        Action action = success ? () => _animator.SetTrigger(_complete) :
            () => _animator.SetTrigger(_skillFail);

        if (success)
        {
            Debug.Log("발사");
            _animationEvent.StartCoroutine(DelayCoroutine(action));
        }
        else
        {
            Debug.Log("스킬취소");
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
        
    }
}

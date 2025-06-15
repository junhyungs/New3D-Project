using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public Animator Animator { get; private set; }
    private Action _reloadSkillAction;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    #region Projectile
    public void SetReloadAction(Action reloadAction)
    {
        Action action = null;
        action = () =>
        {
            reloadAction?.Invoke();
            _reloadSkillAction -= action;
        };

        _reloadSkillAction += action;
    }

    public void UnSetReloadAction()
    {
        if(_reloadSkillAction != null )
            _reloadSkillAction = null;
    }

    public void ReloadSkill()
    {
        _reloadSkillAction?.Invoke();
    }
    #endregion

    public void Test()
    {
        var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(stateInfo.normalizedTime);
    }
}

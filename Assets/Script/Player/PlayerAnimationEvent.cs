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

    public void ReloadSkill()
    {
        _reloadSkillAction?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public Animator Animator { get; private set; }
    private Action _reloadSkillAction;
    private Action _startWeaponAction;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    #region Weapon
    public void SetWeaponAction(Action action)
    {
        _startWeaponAction = action;
    }

    public void UseWeapon()
    {
        _startWeaponAction?.Invoke();
    }

    #endregion
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
}

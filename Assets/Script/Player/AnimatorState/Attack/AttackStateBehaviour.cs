using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using EnumCollection;

public class AttackStateBehaviour : StateMachineBehaviour
{
    protected PlayerHand _hand;
    private bool _isDeactive;
    private const float DEACTIVETIME = 0.5f;
    private readonly int _attackTrigger = Animator.StringToHash("Attack");

    public IAttackStateEventReceiver IAttack { get; set; }
    public Weapon WeaponController { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnEnter();
    }

    protected virtual void OnEnter()
    {
        _isDeactive = false;
        IAttack?.OnAttackAnimEnter();

        WeaponController.DeActiveCurrentWeapon();
        WeaponController.SetWeaponActive(_hand);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnUpdate(stateInfo);
    }

    protected virtual void OnUpdate(AnimatorStateInfo stateInfo)
    {
        bool deactiveCurrentWeapon = !_isDeactive 
            && stateInfo.normalizedTime >= DEACTIVETIME;

        if (deactiveCurrentWeapon)
        {
            _isDeactive = true;
            WeaponController.DeActiveCurrentWeapon();
        }

        bool activeIdleWeapon = stateInfo.normalizedTime >= 1f;
        if (activeIdleWeapon)
        {
            WeaponController.DeActiveCurrentWeapon();
            WeaponController.SetWeaponActive(PlayerHand.Idle);
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExit(animator);
    }

    protected virtual void OnExit(Animator animator)
    {
        IAttack?.OnAttackAnimExit();
        animator.ResetTrigger(_attackTrigger);
    }
}

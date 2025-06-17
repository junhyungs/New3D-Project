using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using EnumCollection;

public class AttackStateBehaviour : StateMachineBehaviour
{
    protected PlayerHand _hand;
    private bool _isDeactive;
    private readonly int _attackTrigger = Animator.StringToHash("Attack");

    public IAttackStateEventReceiver IAttack { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnEnter();
    }

    protected virtual void OnEnter()
    {
        IAttack?.OnAttackAnimEnter(ref _isDeactive, _hand);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnUpdate(stateInfo);
    }

    protected virtual void OnUpdate(AnimatorStateInfo stateInfo)
    {
        IAttack?.OnAttackAnimUpdate(ref _isDeactive, stateInfo);
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

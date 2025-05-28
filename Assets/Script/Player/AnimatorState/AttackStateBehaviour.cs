using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;

public class AttackStateBehaviour : StateMachineBehaviour
{
    public IAttackStateEventReceiver IAttack { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IAttack?.OnAttackAnimEnter();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IAttack?.OnAttackAnimExit();
        animator.ResetTrigger("Attack");
    }
}

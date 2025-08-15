using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class BiteBehaviour : StateMachineBehaviour
    {
        public IBatAttackStateEventReceiver Receiver { get; set; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Receiver.OnAttackAnimEnter();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Receiver.OnAttackAnimUpdate(stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Receiver.OnAttackAnimExit();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class HyperBehaviour : StateMachineBehaviour
    {
        public IStateBehaviourController Controller;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Controller.OnEnter(animator, stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Controller.OnExit(animator, stateInfo);
        }
    }
}


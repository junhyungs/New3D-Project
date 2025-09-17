using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class SlamslowBehaviour : StateMachineBehaviour
    {
        public IStateBehaviourController Controller;        

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Controller.OnEnter(animator, stateInfo);
        }
    }
}


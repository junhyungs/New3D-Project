using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class GhoulAttackBehaviour : StateMachineBehaviour
    {
        public GhoulAttack GhoulAttack { get; set; }
        private Transform _targetTransform;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _targetTransform = GhoulAttack.GetTargetTransform();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_targetTransform == null ||
                stateInfo.normalizedTime > 0.3f)
                return;

            var targetDirection = (_targetTransform.position - animator.transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(targetDirection);

            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation,
               5f * Time.deltaTime);
        }
    }
}


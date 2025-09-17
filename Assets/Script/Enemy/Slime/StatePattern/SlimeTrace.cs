using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class SlimeTrace : SlimeMoveState, ICharacterState<SlimeTrace>
    {
        public SlimeTrace(Slime owner) : base(owner) { }

        public void OnStateEnter()
        {
            SetTargetTransform();
            SetBehaviour();
            PlayAnimation(true);
        }

        public void OnStateExit()
        {
            PlayAnimation(false);   
        }

        private void SetTargetTransform()
        {
            if (_property.TargetTransform != null)
                return;

            var targetTransform = FindPlayer(_property.Data);
            if (targetTransform == null)
                _property.StateMachine.ChangeState(E_SlimeState.Patrol);
            else
                _property.TargetTransform = targetTransform;
        }

        public override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo)
        {
            var target = _property.TargetTransform;
            if (HasReachedDestination(target.position))
            {
                _property.StateMachine.ChangeState(E_SlimeState.Attack);
            }
            else
            {
                SetDestination(target.position);
                AnimationMovement(stateInfo, _property.Data.AgentStopDistance,
                    _property.Data.Speed, _property.Data.Acceleration);
            }
        }
    }
}

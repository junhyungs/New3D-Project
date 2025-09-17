using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class GhoulCoolDown : GhoulState, ICharacterState<GhoulCoolDown>
    {
        public GhoulCoolDown(Ghoul owner) : base(owner) { }
        private WaitForSeconds _coolDownTime = new WaitForSeconds(0.5f);

        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _idleMotion = Animator.StringToHash("IdleMotion");
        private const int MOVE_VALUE = 0;

        private const float MOTION_VALUE = 1f;

        public void OnStateEnter()
        {
            AnimatorSetting();
            _owner.StartCoroutine(CoolDown());
        }

        private void AnimatorSetting()
        {
            var animator = _property.Animator;
            animator.SetFloat(_idleMotion, MOTION_VALUE);
            animator.SetInteger(_move, MOVE_VALUE);
        }

        private IEnumerator CoolDown()
        {
            yield return _coolDownTime;

            var targetTransform = FindPlayer(_property.Data);
            if(targetTransform == null)
            {
                _property.StateMachine.ChangeState(E_GhoulState.Partrol);
                yield break;    
            }

            var distance = Vector3.Distance(targetTransform.position, _owner.transform.position);
            var nextState = distance > _property.NavMeshAgent.stoppingDistance ? 
                E_GhoulState.Trace : E_GhoulState.Attack;
            _property.StateMachine.ChangeState(nextState);
        }
    }
}


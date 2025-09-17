using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class BatTrace : BatState, ICharacterState<BatTrace>, IUnbindAction
    {
        public BatTrace(Bat owner) : base(owner) { }
        private WaitForSeconds _coolDown = new WaitForSeconds(3f);

        private bool _isCoolDown;

        public void AwakeState()
        {
            var attackState = _property.StateMachine.GetState(E_BatState.Attack);
            if (attackState is BatAttack attack)
                attack.StartCoolDown += CoolDown;
        }

        public void Unbind()
        {
            var attackState = _property.StateMachine.GetState(E_BatState.Attack);
            if (attackState is BatAttack attack)
                attack.StartCoolDown -= CoolDown;
        }

        public void OnStateEnter()
        {
            var targetTransform = FindPlayer(_property.Data);
            if (targetTransform == null)
                _property.StateMachine.ChangeState(E_BatState.Patrol);
            else
            {
                var data = _property.Data;
                AgentSetting(data.AgentStopDistance, data.Speed);
                _property.TargetTransform = targetTransform;
            }
        }

        public void OnStateUpdate()
        {
            var targetTransform = _property.TargetTransform;
            var distance = Vector3.Distance(targetTransform.position, _owner.transform.position);
            var canTracking = distance <= _property.Data.DetectionRange;
            if (canTracking)
            {
                bool isStoppingDistance = distance <= _property.NavMeshAgent.stoppingDistance;
                bool canAttack = isStoppingDistance && !_isCoolDown;
                if (canAttack)
                {
                    _isCoolDown = true;
                    _property.NavMeshAgent.SetDestination(_owner.transform.position);
                    _property.StateMachine.ChangeState(E_BatState.Attack);
                }
                else if (isStoppingDistance)
                {
                    Vector3 rotateDirection = (targetTransform.position - _owner.transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(rotateDirection);
                    _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                        lookRotation, 5f * Time.deltaTime);
                }
                else
                    _property.NavMeshAgent.SetDestination(targetTransform.position);
            }
            else
                _property.StateMachine.ChangeState(E_BatState.Patrol);
        }

        private IEnumerator CoolDown()
        {
            yield return _coolDown;
            _isCoolDown = false;
        }

    }
}


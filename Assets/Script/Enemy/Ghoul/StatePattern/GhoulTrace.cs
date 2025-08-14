using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
namespace EnemyComponent
{
    public class GhoulTrace : GhoulState, ICharacterState<GhoulTrace>
    {
        public GhoulTrace(Ghoul owner) : base(owner) { }

        private readonly int _move = Animator.StringToHash("Move");
        private Transform _targetTransform;

        public void OnStateEnter()
        {
            _targetTransform = FindPlayer(_property.Data);
            if (_targetTransform == null)
                _property.StateMachine.ChangeState(E_GhoulState.Partrol);
            else
            {
                var data = _property.Data;
                AgentSetting(data.AgentStopDistance, data.Speed);
                SetInteger();
            }
        }

        public void OnStateUpdate()
        {
            var distance = Vector3.Distance(_targetTransform.position, _owner.transform.position);
            var canTracking = distance <= _property.Data.DetectionRange;
            if (canTracking)
            {
                bool canAttack = distance <= _property.NavMeshAgent.stoppingDistance;
                if (canAttack)
                {
                    _property.NavMeshAgent.SetDestination(_owner.transform.position);
                    _property.StateMachine.ChangeState(E_GhoulState.Attack);
                }
                else
                    _property.NavMeshAgent.SetDestination(_targetTransform.position);
            }
            else
                _property.StateMachine.ChangeState(E_GhoulState.Partrol);
        }

        private void SetInteger() =>
            _property.Animator.SetInteger(_move, 2);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumCollection;

namespace EnemyComponent
{
    public class MageMove : MageState, ICharacterState<MageMove>
    {
        public MageMove(Mage mage) : base(mage) { }
        
        public void OnStateEnter()
        {
            _targetTransform = FindPlayer(_property.Data);
            if (_targetTransform == null)
                _property.StateMachine.ChangeState(E_MageState.Idle);
        }

        public void OnStateUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            var targetDistance = Vector3.Distance(_owner.transform.position, _targetTransform.position);
            bool canAttack = targetDistance <= _property.NavMeshAgent.stoppingDistance;

            if (canAttack)
            {
                _property.NavMeshAgent.SetDestination(_owner.transform.position);
                _property.StateMachine.ChangeState(E_MageState.Attack);
            }
            else
            {
                _property.NavMeshAgent.SetDestination(_targetTransform.position);
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumCollection;

namespace EnemyComponent
{
    public class MageMove : MageState, ICharacterState<MageMove>
    {
        public MageMove(Mage mage) : base(mage)
        {
            _agent = mage.GetComponent<NavMeshAgent>();
            _agent.stoppingDistance = _data.AgentStopDistance;
            _agent.speed = _data.Speed;
        }

        private NavMeshAgent _agent;
        private Transform _targetTransform;

        public void OnStateEnter()
        {
            _targetTransform = FindTarget();
            if (_targetTransform == null)
                _stateMachine.ChangeState(E_MageState.Idle);
        }

        public void OnStateUpdate()
        {
            _agent.SetDestination(_targetTransform.position);
        }
    }
}


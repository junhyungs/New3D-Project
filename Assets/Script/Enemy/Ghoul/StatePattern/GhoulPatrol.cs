using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumCollection;

namespace EnemyComponent
{
    public class GhoulPatrol : Ghoul_CheckPlayer, ICharacterState<GhoulPatrol>
    {
        public GhoulPatrol(Ghoul owner) : base(owner)
        {
            var setting = new PatrolSetting(20, 5f, 1f);
            _patrol = new Patrol(setting, _owner.transform);
        }

        private Patrol _patrol;
        private Vector3 _currentDestination;
        private readonly int _move = Animator.StringToHash("Move");

        public void OnStateEnter()
        {
            _patrol.GeneratePatrolPositions();
            _currentDestination = _patrol.GetDestination;

            StartPatrolTo(_currentDestination);
            InitPlayerScan();
        }

        public void OnStateUpdate()
        {
            if (PlayerScan())
                _property.StateMachine.ChangeState(E_GhoulState.Trace);
            else if(NextPosition(_currentDestination))
                _property.StateMachine.ChangeState(E_GhoulState.Idle);
        }

        private void StartPatrolTo(Vector3 destination)
        {
            _property.NavMeshAgent.SetDestination(destination);
            _property.Animator.SetInteger(_move, 1);

            var data = _property.Data;
            AgentSetting(data.PatrolStoppingDistance, data.PatrolSpeed);
        }

        private bool NextPosition(Vector3 destination)
        {
            return Vector3.Distance(destination, _owner.transform.position) <= 
                _property.NavMeshAgent.stoppingDistance;
        }
    }
}


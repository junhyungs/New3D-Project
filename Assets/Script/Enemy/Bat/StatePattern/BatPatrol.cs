using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class BatPatrol : Bat_CheckPlayer, ICharacterState<BatPatrol>
    {
        public BatPatrol(Bat owner) : base(owner)
        {
            var setting = new PatrolSetting(5, 1f, 1f);
            _patrol = new Patrol(setting, _owner.transform);
        }

        private Patrol _patrol;
        private Vector3 _currentDestination;

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
                _property.StateMachine.ChangeState(E_BatState.Trace);
            else if (NextPosition(_currentDestination))
            {
                if(_patrol.Count <= 0)
                    _patrol.GeneratePatrolPositions();

                _currentDestination = _patrol.GetDestination;
                _property.NavMeshAgent.SetDestination(_currentDestination);
            }
        }

        private void StartPatrolTo(Vector3 destination)
        {
            var data = _property.Data;
            AgentSetting(data.PatrolStoppingDistance, data.PatrolSpeed);
            _property.NavMeshAgent.SetDestination(destination);
        }

        private bool NextPosition(Vector3 destination)
        {
            return Vector3.Distance(destination, _owner.transform.position) <=
                _property.NavMeshAgent.stoppingDistance;
        }
    }
}

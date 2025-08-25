using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

namespace EnemyComponent
{
    public class SlimePatrol : SlimeMoveState, ICharacterState<SlimePatrol>, IStateBehaviourController,
        IInitializeEnable
    {
        public SlimePatrol(Slime owner) : base(owner)
        {
            var setting = new PatrolSetting(20, 4, 3.4f);
            _patrol = new Patrol(setting, _owner.transform);
        }

        private Patrol _patrol;
        private Vector3 _currentDestination;        

        public void OnStateEnter()
        {
            _patrol.GeneratePatrolPositions();
            _currentDestination = _patrol.GetDestination;

            InitPlayerScan();
            SetBehaviour();
            PlayAnimation(true);
        }

        public void OnStateUpdate()
        {
            if (PlayerScan())
                _property.StateMachine.ChangeState(E_SlimeState.Trace);
        }

        public void OnStateExit()
        {
            PlayAnimation(false);
        }

        public override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo)
        {
            bool nextPosition = Vector3.Distance(_currentDestination, _owner.transform.position)
                <= _property.NavMeshAgent.stoppingDistance;

            if (nextPosition)
            {
                if(_patrol.Count <= 0)
                    _patrol.GeneratePatrolPositions();

                _currentDestination = _patrol.GetDestination;
            }

            SetDestination(_currentDestination);
            AnimationMovement(stateInfo, _property.Data.PatrolStoppingDistance,
                _property.Data.PatrolSpeed, _property.Data.Acceleration);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumCollection;

namespace EnemyComponent
{
    public class GhoulPatrol : Ghoul_CheckPlayer, ICharacterState<GhoulPatrol>
    {
        public GhoulPatrol(Ghoul owner) : base(owner) { }
        
        private Queue<Vector3> _patrolQueue = new Queue<Vector3>();
        private List<Vector3> _removeList = new List<Vector3>();

        private Vector3 _currentDestination;

        private const int GRID_SIZE = 20;
        private const float MIN_WALK_DISTANCE = 5f;
        private const float SAMPLE_RADIUS = 0.1f;
        private const float SPHERE_RADIUS = 1f;

        private readonly int _move = Animator.StringToHash("Move");

        public void OnStateEnter()
        {
            if(_patrolQueue.Count <= 0)
            {
                var generatedPositions = GenerateRandomPositions();
                SetWalkPositions(generatedPositions);
            }

            _currentDestination = _patrolQueue.Dequeue();
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

        private List<Vector3> GenerateRandomPositions()
        {
            var list = new List<Vector3>();
            Vector3 lastPosition = _owner.transform.position;

            for(int i = 0; i < 10; i++)
            {
                var positionX = Random.Range(-GRID_SIZE / 2, GRID_SIZE / 2);
                var positionZ = Random.Range(-GRID_SIZE / 2, GRID_SIZE / 2);

                Vector3 newPosition = new Vector3(positionX + _owner.transform.position.x,
                    _owner.transform.position.y, positionZ + _owner.transform.position.z);

                if(Vector3.Distance(newPosition, lastPosition) > MIN_WALK_DISTANCE)
                {
                    list.Add(newPosition);
                    lastPosition = newPosition;
                }
            }

            return list;
        }

        private void SetWalkPositions(List<Vector3> randomPositionList)
        {
            foreach(var position in randomPositionList)
                if(!IsWalkablePosition(position))
                    _removeList.Add(position);

            if(_removeList.Count > 0)
            {
                foreach(var position in _removeList)
                    randomPositionList.Remove(position);

                _removeList.Clear();
            }

            foreach(var position in randomPositionList)
                _patrolQueue.Enqueue(position);
        }

        private bool IsWalkablePosition(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, SAMPLE_RADIUS, NavMesh.AllAreas))
                return false;

            var sphereOrigin = position + Vector3.up;
            return !Physics.CheckSphere(sphereOrigin, SPHERE_RADIUS, LayerMask.GetMask("Object"));
        }
    }
}


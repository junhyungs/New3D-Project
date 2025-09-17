using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyComponent
{
    public struct PatrolSetting
    {
        public int Grid_Size;
        public float MinDistance;
        public float Sample_Radius;
        public float Walkable_Radius;

        public PatrolSetting(int grid_size, float distance, float walkable_radius)
        {
            Grid_Size = grid_size;
            MinDistance = distance;
            Sample_Radius = 0.1f;
            Walkable_Radius = walkable_radius;
        }
    }

    public class Patrol
    {
        public Patrol(PatrolSetting setting, Transform ownerTransform)
        {
            _patrolQueue = new Queue<Vector3>();
            _removeList = new List<Vector3>();
            _setting = setting;
            _ownerTransform = ownerTransform;
        }

        private Queue<Vector3> _patrolQueue;
        private List<Vector3> _removeList;
        private Transform _ownerTransform;
        private PatrolSetting _setting;

        public int Count => _patrolQueue.Count;
        public Vector3 GetDestination => _patrolQueue.Dequeue();

        public void GeneratePatrolPositions()
        {
            if (_patrolQueue.Count > 0)
                return;

            var positions = GenerateRandomPositions();
            SetWalkPositions(positions);
        }

        private List<Vector3> GenerateRandomPositions()
        {
            var list = new List<Vector3>();
            Vector3 lastPosition = _ownerTransform.position;

            var gridSize = _setting.Grid_Size;
            for (int i = 0; i < 10; i++)
            {
                var positionX = Random.Range(-gridSize / 2, gridSize / 2);
                var positionZ = Random.Range(-gridSize / 2, gridSize / 2);

                Vector3 newPosition = new Vector3(positionX + _ownerTransform.position.x,
                    _ownerTransform.position.y, positionZ + _ownerTransform.position.z);

                if (Vector3.Distance(newPosition, lastPosition) > _setting.MinDistance)
                {
                    list.Add(newPosition);
                    lastPosition = newPosition;
                }
            }

            return list;
        }

        private void SetWalkPositions(List<Vector3> list)
        {
            foreach (var position in list)
                if (!IsWalkablePosition(position))
                    _removeList.Add(position);

            if (_removeList.Count > 0)
            {
                foreach (var position in _removeList)
                    list.Remove(position);

                _removeList.Clear();
            }

            foreach (var position in list)
                _patrolQueue.Enqueue(position);
        }

        private bool IsWalkablePosition(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, _setting.Sample_Radius, NavMesh.AllAreas))
                return false;

            var sphereOrigin = position + Vector3.up;
            return !Physics.CheckSphere(sphereOrigin, _setting.Walkable_Radius, LayerMask.GetMask("Object"));
        }
    }
}


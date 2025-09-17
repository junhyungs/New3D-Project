using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "SlimeSO", menuName = "ScriptableObject/Data/SlimeSO")]
    public class SlimeSO : EnemyDataSO
    {
        [Header("Agent_Acceleration")]
        [SerializeField] private float _acceleration;
        [Header("PatrolStopDistance")]
        [SerializeField] private float _patrolStoppingDistance;
        [Header("PatrolSpeed")]
        [SerializeField] private float _patrolSpeed;

        public float PatrolStoppingDistance => _patrolStoppingDistance;
        public float PatrolSpeed => _patrolSpeed;
        public float Acceleration => _acceleration;
    }
}


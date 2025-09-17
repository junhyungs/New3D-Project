using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "GhoulDataSO", menuName = "ScriptableObject/Data/GhoulDataSO")]
    public class GhoulSO : EnemyDataSO
    {
        [Header("PatrolStopDistance")]
        [SerializeField] private float _patrolStoppingDistance;
        [Header("PatrolSpeed")]
        [SerializeField] private float _patrolSpeed;

        public float PatrolStoppingDistance => _patrolStoppingDistance;
        public float PatrolSpeed => _patrolSpeed;
    }
}


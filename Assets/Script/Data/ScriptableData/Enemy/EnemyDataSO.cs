using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class EnemyDataSO : ScriptableData
    {
        [Header("Health")]
        [SerializeField] private int _health;
        [Header("Damage")]
        [SerializeField] private int _damage;
        [Header("Speed")]
        [SerializeField] private float _speed;
        [Header("AgentStopDistance")]
        [SerializeField] private float _agentStopDistance;
        [Header("DetectionRange")]
        [SerializeField] private float _detectionRange;
        [Header("Spawn_DetectionRange")]
        [SerializeField] private float _spawn_detectionRange;

        public override ScriptableDataKey Key => _key;
        public int Health => _health;
        public int Damage => _damage;
        public float Speed => _speed;
        public float AgentStopDistance => _agentStopDistance;
        public float DetectionRange => _detectionRange;
        public float Spawn_DetectionRange => _spawn_detectionRange;
    }
}


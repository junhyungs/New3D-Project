using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public abstract class MonsterDataSO : ScriptableData
    {
        [Header("Health")]
        [SerializeField] private int _health;
        [Header("Damage")]
        [SerializeField] private int _damage;
        [Header("DetectionRange")]
        [SerializeField] private float _detectionRange;
        [Header("Spawn_DetectionRange")]
        [SerializeField] private float _spawn_detectionRange;

        public int Health => _health;
        public int Damage => _damage;
        public float DetectionRange => _detectionRange;
        public float Spawn_DetectionRange => _spawn_detectionRange;
    }


    public class EnemyDataSO : MonsterDataSO
    {
        [Header("Speed")]
        [SerializeField] private float _speed;
        [Header("AgentStopDistance")]
        [SerializeField] private float _agentStopDistance;
        

        public override ScriptableDataKey Key => _key;
        public float Speed => _speed;
        public float AgentStopDistance => _agentStopDistance;
    }

    public class BossDataSO : MonsterDataSO
    {
        public override ScriptableDataKey Key => _key;
    }
}


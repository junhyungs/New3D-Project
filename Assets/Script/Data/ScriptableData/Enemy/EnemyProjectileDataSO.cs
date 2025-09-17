using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class EnemyProjectileDataSO : ScriptableObject
    {
        [Header("Speed")]
        [SerializeField] private float _speed;
        public float Speed => _speed;
    }
}


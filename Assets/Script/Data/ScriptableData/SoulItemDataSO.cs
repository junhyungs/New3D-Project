using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "SoulItemDataSO", menuName = "ScriptableObject/Data/SoulItemDataSO")]
    public class SoulItemDataSO : ItemDataSO
    {
        [Header("Movement")]
        [SerializeField] private float _maxTime;
        [SerializeField] private float _moveSpeed;

        [Header("Soul")]
        [SerializeField] private int _soulValue;

        public float MaxTime => _maxTime;
        public float MoveSpeed => _moveSpeed;
        public int SoulValue => _soulValue;
    }
}


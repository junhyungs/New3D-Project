using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "PlayerSkillDataSO", menuName = "ScriptableObject/Data/PlayerSkillDataSO")]
    public class PlayerSkillDataSO : ScriptableObject
    {
        [Header("ProjectileSpeed"), SerializeField]
        private float _projectileSpeed;
        [Header("Damage"), SerializeField]
        private int _damage;
        [Header("Cost"), SerializeField]
        private int _cost;
        [Header("FlightTime"), SerializeField]
        private float _flightTime;

        public float ProjectileSpeed => _projectileSpeed;
        public int Damage => _damage;
        public int Cost => _cost;
        public float FlightTime => _flightTime;
    }
}


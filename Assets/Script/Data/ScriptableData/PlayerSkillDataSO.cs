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

        public SkillData SkillData => new SkillData(_projectileSpeed, 
            _damage, _cost, _flightTime);
    }
}


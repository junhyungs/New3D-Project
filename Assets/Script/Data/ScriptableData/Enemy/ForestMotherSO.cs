using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "ForestMotherSO", menuName = "ScriptableObject/Data/ForestMotherSO")]
    public class ForestMotherSO : BossDataSO
    {
        [Header("LiftTime")]
        [SerializeField] private float _liftTime;
        [Header("VineHealth")]
        [SerializeField] private int _vineHealth;
        public float LiftTime => _liftTime;
        public int VineHealth => _vineHealth;
    }
}


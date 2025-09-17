using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "ForestMotherProjectileSO", menuName = "ScriptableObject/Data/ForestMotherProjectileSO")]
    public class ForestMotherProjectileSO : EnemyProjectileDataSO
    {
        [Header("ExplosionRadius")]
        [SerializeField] private float _explosionRadius;
        [Header("ExplosionDamage")]
        [SerializeField] private int _explosionDamage;
        public float ExplosionRadius => _explosionRadius;
        public int ExplosionDamage => _explosionDamage;
    }
}


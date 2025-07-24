using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{

    [CreateAssetMenu(fileName = "MageDataSO", menuName = "ScriptableObject/Data/MageDataSO")]
    public class MageSO : EnemyDataSO
    {
        [Header("TeleportDistance")]
        [SerializeField]
        private float _teleportDistance;

        public float TeleportDistance => _teleportDistance;
    }
}


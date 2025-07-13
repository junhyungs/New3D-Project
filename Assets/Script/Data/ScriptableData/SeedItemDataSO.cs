using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "SeedItemDataSO", menuName = "ScriptableObject/Data/SeedItemDataSO")]
    public class SeedItemDataSO : ItemDataSO
    {
        [Header("Count"), SerializeField]
        private int _count;

        public int Count => _count;
    }
}


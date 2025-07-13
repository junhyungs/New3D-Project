using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "PlayerWeaponDataSO", menuName = "ScriptableObject/Data/PlayerWeaponDataSO")]
    public class PlayerWeaponDataSO : InventoryItemDataSO
    {
        [Header("Damage"), SerializeField]
        private int _damage;
        [Header("Range"), SerializeField]
        private Vector3 _range;

        public WeaponData GetWeaponData()
        {
            return new WeaponData(_damage, _range);
        }
    }
}


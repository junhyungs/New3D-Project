using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "PlayerWeaponDataSO", menuName = "ScriptableObject/Data/PlayerWeaponDataSO")]
    public class PlayerWeaponDataSO : ItemDataSO
    {
        [Header("AddressKey"), SerializeField]
        private string _addressKey;
        [Header("Damage"), SerializeField]
        private int _damage;
        [Header("Range"), SerializeField]
        private Vector3 _range;
        [Header("WeaponType"), SerializeField]
        private ItemType _itemType;
        public WeaponData WeaponData => new WeaponData(_damage, _range, ItemName, ItemDescription, _itemType);
        public string AddressKey => _addressKey;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using SO;
using EnumCollection;

namespace PlayerComponent
{
    [System.Serializable]
    public class EquipTransformInfo
    {
        [Header("EquipTransform")]
        public Transform Holster;
        public Transform WeaponL;
        public Transform WeaponR;
    }

    public class PlayerWeaponSystem : MonoBehaviour
    {
        [Header("WeaponTransform"), SerializeField]
        private WeaponTransform[] _weaponTransforms;

        [Header("EquipTransform"), SerializeField]
        private EquipTransformInfo _equipTransformInfo;

        private Dictionary<ItemType, Dictionary<PlayerHand, WeaponTransform>> _weaponTransformDictionary 
            = new Dictionary<ItemType, Dictionary<PlayerHand, WeaponTransform>>();

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach(var weaponTransform in _weaponTransforms)
            {
                var weaponType = weaponTransform.WeaponType;
                var handType = weaponTransform.HandType;

                if(!_weaponTransformDictionary.TryGetValue(weaponType, out var handDictionary))
                {
                    handDictionary = new Dictionary<PlayerHand, WeaponTransform>();
                    _weaponTransformDictionary.Add(weaponType, handDictionary);
                }

                handDictionary.TryAdd(handType, weaponTransform);
            }
        }

        public Dictionary<PlayerHand, WeaponTransform> GetWeaponTransformDictionary(ItemType type)
        {
            if (!_weaponTransformDictionary.TryGetValue(type, out var handDictionary))
                return null;

            return handDictionary;
        }
    }
}


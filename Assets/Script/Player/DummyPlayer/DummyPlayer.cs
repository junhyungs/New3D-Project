using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class DummyPlayer : MonoBehaviour
    {
        [System.Serializable]
        public class DummyWeaponInfo
        {
            public ItemType Type;
            public GameObject HolsterWeapon;
        }

        [Header("HolsterWeapon"), SerializeField]
        private DummyWeaponInfo[] _weapons;
        private Dictionary<ItemType, GameObject> _weaponDictionary = new Dictionary<ItemType, GameObject>();

        private void Awake()
        {
            foreach(var weapon in _weapons)
                if(weapon != null)
                    _weaponDictionary.Add(weapon.Type, weapon.HolsterWeapon);
        }

        private void OnEnable()
        {
            var weaponManager = WeaponManager.Instance;
            
            AllDisableWeapon();
            var weaponObject = GetHolsterWeapon(weaponManager.WeaponType);
            weaponObject.SetActive(true);
        }

        private GameObject GetHolsterWeapon(ItemType type)
        {
            return _weaponDictionary[type];
        }

        private void AllDisableWeapon()
        {
            foreach(var weapon in _weaponDictionary.Values)
                if(weapon != null)
                    weapon.SetActive(false);
        }
    }
}


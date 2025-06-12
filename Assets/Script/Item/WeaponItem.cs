using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class WeaponItem : Item, IPlayerWeaponItem
    {
        [Header("SlotName"), SerializeField]
        private ItemType _slotName;
        [Header("DataKey")]
        [SerializeField] private DataKey _descriptionKey;
        [SerializeField] private DataKey _weaponDataKey;
        [SerializeField] private ObjectKey _prefabKey;

        public bool CanEquip => true;
        public ItemType SlotName => _slotName;
        public string DescriptionKey => _descriptionKey.ToString();
        public string WeaponDataKey => _weaponDataKey.ToString();
        public string PrefabKey => _prefabKey.ToString();

        public override void Interact()
        {
            InventroyManager.Instance.SetItem(this);
            DisableObejct();
        }
    }
}


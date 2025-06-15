using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class WeaponItem : Item, IPlayerWeaponItem
    {
        [Header("SlotName"), SerializeField]
        private ItemType _slotName;
        [Header("DataKey")]
        [SerializeField] private DataKey _descriptionKey;
        [SerializeField] private DataKey _weaponDataKey;

        public override bool CanEquip => true;
        public override ItemType SlotName => _slotName;
        public override string DescriptionKey => _descriptionKey.ToString();
        public string WeaponDataKey => _weaponDataKey.ToString();

        public override void Interact()
        {
            InventroyManager.Instance.SetItem(this);
            DisableObejct();
        }
    }
}


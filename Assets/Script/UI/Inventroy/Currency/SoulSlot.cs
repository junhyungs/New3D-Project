using EnumCollection;
using GameData;
using ItemComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI
{
    public class SoulSlot : CurrencyItemSlot
    {
        private void Start()
        {
            _key = UIEvent.SoulView.ToString();
            InventoryManager.Instance.RegisterSlot(ItemType.Soul, this);
        }

        public override int Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                UIManager.TriggerUIEvent(_key, _currency);
            }
        }

        public override void SaveCurrency(PlayerInventoryData inventoryData)
        {
            inventoryData.SoulCount = _currency;
        }
    }
}


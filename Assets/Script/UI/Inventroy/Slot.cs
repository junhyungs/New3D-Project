using GameData;
using System.Collections.Generic;
using UnityEngine;
using ItemComponent;

namespace InventoryUI
{   
    public abstract class PlayerItemSlot : MonoBehaviour
    {
        public abstract void InitializeSlot();
    }

    public abstract class CurrencyItemSlot : PlayerItemSlot
    {
        protected string _key;
        protected int _currency;
        public abstract int Currency { get; set; }
        public virtual bool CanUseCurrencyItem(int count)
        {
            var carculate = Currency - count;
            if (carculate < 0)
                return false;
            return true;
        }

        public virtual void UseItem(int count)
        {
            Currency -= count;
        }

        public override void InitializeSlot() { }
        public abstract void SaveCurrency(PlayerInventoryData inventoryData);        
    }
}


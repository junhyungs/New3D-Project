using EnumCollection;
using ItemComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI
{
    public class SeedSlot : CurrencyItemSlot
    {
        public SeedSlot()
        {
            InitializeSlot();
        }

        public override int Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                TriggerUIEvent();
            }
        }

        public override void InitializeSlot()
        {
            _key = UIEvent.SeedView.ToString();
            TriggerUIEvent();
        }

        public override bool CanUseCurrencyItem(int count)
        {
            var carculate = Currency - count;
            if(carculate < 0)
                return false;
            return true;
        }

        public override void UseItem(int count)
        {
            Currency -= count;
        }
    }
}


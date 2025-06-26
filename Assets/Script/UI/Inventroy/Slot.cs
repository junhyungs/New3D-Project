
using GameData;
using System.Collections.Generic;
using UnityEngine;
using ItemComponent;

namespace InventoryUI
{
    public interface ISlot
    {
        void InitializeSlot();
    }

    public abstract class CurrencyItemSlot : ISlot
    {
        public enum CurrencySlot
        {
            Seed,
            Soul
        }

        protected string _key;
        protected int _currency;
        public abstract int Currency { get; set; }
        public abstract CurrencySlot GetSlotType { get; }
        public abstract bool CanUseCurrencyItem(int count);
        public abstract void UseItem(int count);
        public abstract void InitializeSlot();
        protected void TriggerUIEvent()
        {
            UIManager.TriggerUIEvent(_key, _currency);
        }
    }

    public abstract class PlayerItemSlot : MonoBehaviour, ISlot
    {
        protected ItemDescriptionData _descriptionData;
        public abstract ItemDescriptionData DescriptionData { get; set; }
        public abstract void InitializeSlot();
    }

    public abstract class WeaponItemSlot : PlayerItemSlot
    {
        protected PlayerWeaponData _weaponData;
        public abstract PlayerWeaponData WeaponData { get; set; }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace ItemComponent
{
    public interface IGameItem
    {
        ItemType SlotName { get; }
    }

    public interface ICurrencyItem : IGameItem
    {
        int GetValue();
    }
    
    public interface IPlayerItem : IGameItem
    {
        bool CanEquip { get; }
        string DescriptionKey { get; }
    }

    public interface IPlayerWeaponItem : IPlayerItem
    {
        string WeaponDataKey { get; }
        string AddressableKey { get; }
    }

    public abstract class CurrencyItem : MonoBehaviour, IInteractionItem, ICurrencyItem
    {
        protected SphereCollider _collider;
        public abstract ItemType SlotName { get; }
        public abstract int GetValue();
        public abstract void Interact();

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }
    }

    public abstract class Item : MonoBehaviour, IInteractionItem, IPlayerItem
    {
        protected SphereCollider _collider;
        public abstract bool CanEquip { get; }
        public abstract ItemType SlotName { get; }
        public abstract string DescriptionKey { get; }

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void OnEnable()
        {
            _collider.enabled = true;
        }

        protected void DisableObejct()
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }

        public abstract void Interact();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

namespace ItemComponent
{
    public interface IGameItem
    {
        ItemType SlotName { get; }
    }

    public interface IInventoryItem : IGameItem
    {
        ItemDataSO ItemDataSO { get; }
    }

    public interface ICurrencyItem : IInventoryItem
    {
        int GetValue();
    }
   
    public abstract class CurrencyItem : MonoBehaviour, IInteractionItem
    {
        protected SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }
        protected void DisableObejct()
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }

        public abstract void Interact();
    }

    public abstract class Item : MonoBehaviour, IInteractionItem, IInventoryItem
    {
        [Header("InventoryItemDataSO"), SerializeField]
        private ItemDataSO _itemDataSO;
        protected SphereCollider _collider;

        public ItemDataSO ItemDataSO => _itemDataSO;
        public abstract ItemType SlotName { get; }

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        protected void DisableObejct()
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }

        public abstract void Interact();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace Item
{
    public interface IPlayerItem
    {
        bool CanEquip { get; }
        ItemType SlotName { get; }
        string DescriptionKey { get; }
    }

    public interface IPlayerWeaponItem : IPlayerItem
    {
        string WeaponDataKey { get; }
        string PrefabKey { get; }
    }

    public abstract class Item : MonoBehaviour, IInteractionItem
    {
        protected SphereCollider _collider;

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

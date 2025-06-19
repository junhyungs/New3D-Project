using GameData;
using UnityEngine;
using EnumCollection;
using UnityEngine.UI;

namespace InventoryUI
{
    public class WeaponSlot : WeaponItemSlot
    {
        [Header("ItemName"), SerializeField]
        private ItemType _itemName;
        [Header("WeaponTexture"), SerializeField]
        private Texture2D _weaponTexture;
        [Header("RenderTexture"), SerializeField]
        private RenderTexture _renderTexture;
        [Header("RawImage"), SerializeField]
        private RawImage _rawImage;

        private const string KEY = "EnableItem";

        public override PlayerWeaponData WeaponData { get; set; }
        public override ItemDescriptionData DescriptionData { get; set; }

        private void Awake()
        {
            InventoryManager.Instance.RegisterSlot(_itemName, this);
        }

        private void OnDisable()
        {
            ChangeImage(false);
        }

        public override void InitializeSlot()
        {
            _rawImage.enabled = true;
            _rawImage.texture = _weaponTexture;
        }

        public void LiveImage()
        {
            ChangeImage(true);
        }

        public void CaptureImage()
        {
            ChangeImage(false);
        }

        private void ChangeImage(bool isLive)
        {
            UIManager.TriggerUIEvent(KEY, (_itemName, isLive));
            _rawImage.texture = isLive ? _renderTexture : _weaponTexture;
        }

        public void ChangeWeapon()
        {
            bool isData = WeaponData != null
                && DescriptionData != null;

            if (!isData)
                return;

            WeaponManager.Instance.SetWeapon(_itemName);
        }
    }
}


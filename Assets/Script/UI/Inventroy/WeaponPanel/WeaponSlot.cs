using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEngine.UI;

namespace InventoryUI
{
    public class WeaponSlot : WeaponDataSlot
    {
        [Header("ItemName"), SerializeField]
        private InventoryItem _itemName;
        [Header("WeaponTexture"), SerializeField]
        private Texture2D _weaponTexture;
        [Header("RenderTexture"), SerializeField]
        private RenderTexture _renderTexture;
        [Header("RawImage"), SerializeField]
        private RawImage _rawImage;

        private const string KEY = "WeaponItem";

        public override PlayerWeaponData WeaponData { get; set; }
        public override ItemDescriptionData DescriptionData { get; set; }

        private void Awake()
        {
            InventroyManager.Instance.RegisterSlot(_itemName, this);
        }

        public void InitializeWeaponSlot()
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
    }
}


using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryUI
{
    public class TrinketSlot : PlayerItemSlot
    {
        [Header("ItemName"), SerializeField]
        private ItemType _itemName;
        [Header("TrinketTexture"), SerializeField]
        private Texture2D _trinketTexture;
        [Header("RenderTexture"), SerializeField]
        private RenderTexture _renderTexture;
        [Header("RawImage"), SerializeField]
        private RawImage _rawImage;

        private const string KEY = "EnableItem";

        public override ItemDescriptionData DescriptionData { get; set; }

        public override void InitializeSlot()
        {
            _rawImage.enabled = true;
            _rawImage.texture = _trinketTexture;
        }

        private void Awake()
        {
            InventoryManager.Instance.RegisterSlot(_itemName, this);
        }

        private void OnDisable()
        {
            ChangeImage(false);
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
            _rawImage.texture = isLive ? _renderTexture : _trinketTexture;
        }

    }
}


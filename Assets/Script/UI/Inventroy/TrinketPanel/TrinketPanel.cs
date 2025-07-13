using EnumCollection;
using GameData;
using ModelViewPresenter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventoryUI
{
    public class TrinketPanel : Panel<TrinketPresenter>, ITrinketView
    {
        private void Awake()
        {
            _presenter = new TrinketPresenter(this);
        }

        protected override void OnEnablePanel()
        {
            base.OnEnablePanel();
            StartCoroutine(WaitForCurrentSelectedGameObject());
        }

        protected override void SlotControl(InputAction.CallbackContext context)
        {
            StartCoroutine(WaitForCurrentSelectedGameObject());
        }

        public void UpdateDescription(TrinketItemData data)
        {
            InitializeText();

            if (data == null)
                return;

            var description = data.ItemDescription;
            description = description.Replace("\\n", "\n");

            _descriptionNameText.text = data.ItemName;
            _descriptionText.text = description;
        }
    }
}


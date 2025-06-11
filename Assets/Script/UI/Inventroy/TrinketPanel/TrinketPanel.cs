using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;
using UnityEngine.InputSystem;
using ModelViewPresenter;
using GameData;

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

        public void UpdateDescription(ItemDescriptionData data)
        {
            InitializeText();

            if (data == null)
                return;

            _descriptionNameText.text = data.ItemName;
            _descriptionText.text = data.Description;
        }
    }
}


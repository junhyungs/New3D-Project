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
        public void UpdateDescription(ItemDescriptionData data)
        {
            
        }

        protected override void SlotControl(InputAction.CallbackContext context)
        {
            
        }
    }
}


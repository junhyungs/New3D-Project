using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;
using UnityEngine.InputSystem;

namespace InventroyUI
{
    public class TrinketPanel : Panel<TrinketPanelType>
    {
        [Header("Slots"), SerializeField]
        private GameObject[] _slots;

        protected override void InitializeOnAwake()
        {
            var enumArray = (TrinketPanelType[])Enum.GetValues(typeof(TrinketPanelType));
            _infos = new Info<TrinketPanelType>[enumArray.Length];
            for (int i = 0; i < enumArray.Length; i++)
            {
                var type = enumArray[i];
                if (_slots[i] != null)
                {
                    _slotTypeDictionary.Add(_slots[i], type);

                    _infos[i] = new Info<TrinketPanelType>();
                    _infos[i].Type = type;
                    _infos[i].SlotObject = _slots[i];
                }
            }
        }

        protected override void SlotControl(InputAction.CallbackContext context)
        {
            
        }
    }
}


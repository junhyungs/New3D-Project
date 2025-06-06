using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventroyUI
{
    public class IconController : MonoBehaviour
    {
        [Header("HorizontalAction"), SerializeField]
        private InputActionReference _horizontalAction;

        [Header("IconInfo"), SerializeField]
        private IconInfo[] _iconInfos;

        private IconInfo _current;
        private int _iconIndex;

        private void Start()
        {
            MoveIcon(-1);
        }

        private void OnEnable()
        {
            _horizontalAction.action.Enable();
            _horizontalAction.action.performed += IconControl;
        }

        private void OnDisable()
        {
            _horizontalAction.action.performed -= IconControl;
            _horizontalAction.action.Disable();
        }

        private void IconControl(InputAction.CallbackContext context)
        {
            bool zKey = Keyboard.current.zKey.wasPressedThisFrame;
            bool xKey = Keyboard.current.xKey.wasPressedThisFrame;

            PanelEnable(false, _current);

            if (zKey)
            {
                MoveIcon(-1);
            }
            else if (xKey)
            {
                MoveIcon(1);
            }
        }

        private void MoveIcon(int direction)
        {
            _iconIndex = Mathf.Clamp(_iconIndex + direction, 0, _iconInfos.Length - 1);

            var nextIcon = _iconInfos[_iconIndex];
            if (nextIcon == null)
                return;

            PanelEnable(true, nextIcon);
        }

        private void PanelEnable(bool enable, IconInfo iconInfo)
        {
            if (iconInfo == null)
                return;

            var alphaValue = enable ? 1f : 0.3f;

            var iconColor = iconInfo.Icon.color;
            iconColor.a = alphaValue;
            iconInfo.Icon.color = iconColor;

            iconInfo.Panel.SetActive(enable);
            _current = iconInfo;
        }
    }
}


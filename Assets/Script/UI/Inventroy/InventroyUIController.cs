using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventroyUI
{
    public class InventroyUIController : MonoBehaviour
    {
        [Header("QAction"), SerializeField]
        private InputActionReference _qAction;
        private GameObject _inventory;

        private void Awake()
        {
            _inventory = transform.GetChild(0).gameObject;
        }

        private void OnEnable()
        {
            _qAction.action.Enable();
            _qAction.action.performed += OpenInventroy;
        }

        private void OnDisable()
        {
            _qAction.action.performed -= OpenInventroy;
            _qAction.action.Disable();
        }

        private void OpenInventroy(InputAction.CallbackContext context)
        {
            var enabled = _inventory.activeSelf ? false : true;
            
            PlayerInputSetting(!enabled);
            SetTimeScale(enabled);

            UIManager.Instance.MovePlayerInfoUI(enabled);
            _inventory.SetActive(enabled);
        }

        private void PlayerInputSetting(bool enabled)
        {
            var playerComponent = PlayerManager.Instance.PlayerComponent;
            if(playerComponent != null)
                playerComponent.InputHandler.EnabledPlayerInput(enabled);
        }

        private void SetTimeScale(bool enabled)
        {
            var timeScale = enabled ? 0f : 1f;
            Time.timeScale = timeScale;
        }
    }
}


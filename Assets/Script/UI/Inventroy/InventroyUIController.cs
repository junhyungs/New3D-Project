using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        bool enable = _inventory.activeSelf ? false : true;
        _inventory.SetActive(enable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using ModelViewPresenter;

namespace InventoryUI
{
    public class Panel<T> : MonoBehaviour where T : Presenter
    {
        [Header("SlotControlAction"), SerializeField]
        private InputActionReference _controlAction;

        [Header("FirstObject"), SerializeField]
        private GameObject _firstGameObject;

        [Header("Description")]
        [SerializeField] protected TextMeshProUGUI _descriptionNameText;
        [SerializeField] protected TextMeshProUGUI _descriptionText;

        protected T _presenter;

        protected virtual void OnEnable()
        {
            _controlAction.action.Enable();
            _controlAction.action.performed += SlotControl;

            OnEnablePanel();
        }

        protected virtual void OnDisable()
        {
            _controlAction.action.performed -= SlotControl;
            _controlAction.action.Disable();
        }
            
        protected virtual void SlotControl(InputAction.CallbackContext context) { }
        
        protected virtual void OnEnablePanel()
        {
            SetSelectedGameObject(_firstGameObject);
        }

        protected virtual IEnumerator WaitForCurrentSelectedGameObject()
        {
            yield return null;

            var currentObject = EventSystem.current.currentSelectedGameObject;
            _presenter.RequestUpdate(currentObject);
        }

        protected void SetSelectedGameObject(GameObject gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        protected void InitializeText()
        {
            _descriptionNameText.text = string.Empty;
            _descriptionText.text = string.Empty;
        }
    }
}


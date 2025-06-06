using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using GameData;
using TMPro;
namespace InventroyUI
{
    public class Panel<T> : MonoBehaviour where T : Enum
    {
        [Header("SlotControlAction"), SerializeField]
        private InputActionReference _controlAction;

        [Header("Description")]
        [SerializeField] protected TextMeshProUGUI _descriptionNameText;
        [SerializeField] protected TextMeshProUGUI _descriptionText;

        [Header("Info"), SerializeField]
        protected Info<T>[] _infos;

        protected Dictionary<GameObject, T> _slotTypeDictionary = new Dictionary<GameObject, T>();
        protected Dictionary<T, ItemDescriptionData> _dataDictionary = new Dictionary<T, ItemDescriptionData>();

        protected virtual void Awake()
        {
            InitializeOnAwake();
        }

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

        protected virtual void InitializeOnAwake()
        {
            foreach (var info in _infos)
                if (info.SlotObject != null)
                    _slotTypeDictionary.Add(info.SlotObject, info.Type);
        }
            
        protected virtual void SlotControl(InputAction.CallbackContext context) { }

        protected void OnEnablePanel()
        {
            var firstObject = _infos[0].SlotObject;
            if (firstObject == null)
                return;

            SetSelectedGameObject(firstObject);
        }

        protected void SetSelectedGameObject(GameObject gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}


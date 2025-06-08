using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEngine.InputSystem;
using ModelViewPresenter;
using GameData;
using TMPro;

namespace InventoryUI
{
    public class WeaponPanel : Panel<WeaponPresenter>, IWeaponView
    {
        [Header("AbilityUI")]
        [SerializeField] private TextMeshProUGUI _damage;
        [SerializeField] private TextMeshProUGUI _range;
        [SerializeField] private TextMeshProUGUI _hit;
        [SerializeField] private TextMeshProUGUI _speed;
       
        private void Awake()
        {
            _presenter = new WeaponPresenter(this);
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

        public void UpdateWeaponAbility(PlayerWeaponData data)
        {
            InitializeText();

            if (data == null)
                return;

            _damage.text = string.Empty;
            _range.text = string.Empty;
            _hit.text = string.Empty;
            _speed.text = string.Empty;
        }
    }
}


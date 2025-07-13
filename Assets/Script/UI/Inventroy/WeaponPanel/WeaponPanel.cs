using EnumCollection;
using GameData;
using ModelViewPresenter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventoryUI
{
    public class WeaponPanel : Panel<WeaponPresenter>, IWeaponView
    {
        [Header("AbilityUI")]
        [SerializeField] private TextMeshProUGUI _damage;
        [SerializeField] private TextMeshProUGUI _range;
        [SerializeField] private TextMeshProUGUI _hit;
        [SerializeField] private TextMeshProUGUI _speed;
        private TextMeshProUGUI[] _abilityUI;

        private void Awake()
        {
            _presenter = new WeaponPresenter(this);
            _abilityUI = new TextMeshProUGUI[] {_damage, _range, _hit, _speed};
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

        public void UpdateWeaponAbility(WeaponData data)
        {
            foreach (var ui in _abilityUI)
                ui.text = string.Empty;

            if (data == null)
                return;

            _damage.text = data.Damage.ToString();
            _range.text = data.Range.x.ToString();
            _hit.text = (data.Damage + 1).ToString();
            _speed.text = 0.4.ToString();
        }

        public void UpdateDescription(WeaponData data)
        {
            InitializeText();

            if (data == null)
                return;

            var description = data.ItemDescription;
            description = description.Replace("\\n", "\n");

            _descriptionNameText.text = data.WeaponName;
            _descriptionText.text = description;
        }
    }
}


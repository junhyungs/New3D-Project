using System.Collections;
using UnityEngine.InputSystem;
using ModelViewPresenter;
using GameData;
using UnityEngine.EventSystems;

namespace InventoryUI
{
    public class InventoryPanel : Panel<InventoryPresenter>, IInventoryView
    {
        private void Awake()
        {
            _presenter = new InventoryPresenter(this);
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


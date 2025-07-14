using GameData;
using ModelViewPresenter;
using UnityEngine.InputSystem;

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

        public void UpdateDescription(InventoryItemData data)
        {
            InitializeText();

            if (data == null)
                return;

            var description = data.ItemDescription;
            description = description.Replace("\\n", "\n");

            _descriptionNameText.text = data.ItemName;
            _descriptionText.text = description;
        }
    }
}


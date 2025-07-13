using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewPresenter;

namespace InventoryUI
{
    public class InventoryPresenter : Presenter<IInventoryView>
    {
        public InventoryPresenter(IInventoryView view) : base(view) { }

        protected override void UpdateView(PlayerItemSlot slot)
        {
            var inventorySlot = slot as InventorySlot;
            if(inventorySlot == null)
            {
                _view.UpdateDescription(null);
                return;
            }

            var inventoryData = inventorySlot.InventoryItemData;

            bool isData = inventoryData != null;
            if (isData)
            {
                _view.UpdateDescription(inventoryData);
            }
            else
                _view.UpdateDescription(null);
        }
    }
}


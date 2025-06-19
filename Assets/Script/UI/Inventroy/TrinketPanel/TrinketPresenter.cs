using GameData;
using ModelViewPresenter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI
{
    public class TrinketPresenter : Presenter<ITrinketView>
    {
        public TrinketPresenter(ITrinketView view) : base(view) { }
        private TrinketSlot _previousSlot;

        protected override void UpdateView(PlayerItemSlot slot)
        {
            if (_previousSlot != null)
                _previousSlot.CaptureImage();

            var trinketSlot = slot as TrinketSlot;
            if(trinketSlot == null)
            {
                UpdateTrinketView();
                return;
            }

            var itemDescriptionData = trinketSlot.DescriptionData;

            bool isData = itemDescriptionData != null;
            if (isData)
            {
                trinketSlot.LiveImage();
                UpdateTrinketView(itemDescriptionData);

                _previousSlot = trinketSlot;
            }
            else
                UpdateTrinketView();
        }

        private void UpdateTrinketView(ItemDescriptionData itemData = null)
        {
            _view.UpdateDescription(itemData);
        }
    }
}


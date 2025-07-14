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

            var trinketItemData = trinketSlot.TrinketItemData;

            bool isData = trinketItemData != null;
            if (isData)
            {
                trinketSlot.LiveImage();
                UpdateTrinketView(trinketItemData);

                _previousSlot = trinketSlot;
            }
            else
                UpdateTrinketView();
        }

        private void UpdateTrinketView(TrinketItemData data = null)
        {
            _view.UpdateDescription(data);
        }
    }
}


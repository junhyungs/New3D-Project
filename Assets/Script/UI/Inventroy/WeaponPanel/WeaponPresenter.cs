using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewPresenter;
using GameData;

namespace InventoryUI
{
    public class WeaponPresenter : Presenter<IWeaponView>
    {
        public WeaponPresenter(IWeaponView view) : base(view) { }
        private WeaponSlot _previousSlot;

        protected override void UpdateView(Slot slot)
        {
            if(_previousSlot != null)
                _previousSlot.CaptureImage();

            var weaponSlot = slot as WeaponSlot;
            if(weaponSlot == null)
            {
                UpdateWeaponView();
                return;
            }
                
            var itemDescriptionData = weaponSlot.DescriptionData;
            var weaponData = weaponSlot.WeaponData;

            bool isData = itemDescriptionData != null &&
                weaponData != null;

            if (isData)
            {
                weaponSlot.LiveImage();
                UpdateWeaponView(itemDescriptionData, weaponData);

                _previousSlot = weaponSlot;
            }
            else
                UpdateWeaponView();
        }

        private void UpdateWeaponView(ItemDescriptionData itemData = null,
            PlayerWeaponData weaponData = null)
        {
            _view.UpdateDescription(itemData);
            _view.UpdateWeaponAbility(weaponData);
        }
    }
}


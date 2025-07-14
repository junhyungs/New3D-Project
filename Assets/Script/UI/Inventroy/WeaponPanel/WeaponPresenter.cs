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

        protected override void UpdateView(PlayerItemSlot slot)
        {
            if(_previousSlot != null)
                _previousSlot.CaptureImage();

            var weaponSlot = slot as WeaponSlot;
            if(weaponSlot == null)
            {
                UpdateWeaponView();
                return;
            }
                
            var weaponData = weaponSlot.WeaponData;

            bool isData = weaponData != null;
            if (isData)
            {
                weaponSlot.LiveImage();
                UpdateWeaponView(weaponData);

                _previousSlot = weaponSlot;
            }
            else
                UpdateWeaponView();
        }

        private void UpdateWeaponView(WeaponData data = null)
        {
            _view.UpdateDescription(data);
            _view.UpdateWeaponAbility(data);
        }
    }
}


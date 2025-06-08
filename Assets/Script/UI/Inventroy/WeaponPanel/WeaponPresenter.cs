using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewPresenter;

namespace InventoryUI
{
    public class WeaponPresenter : Presenter<IWeaponView>
    {
        public WeaponPresenter(IWeaponView view) : base(view) { }
        protected override void UpdateView(Slot slot)
        {
            var data = slot as WeaponDataSlot;

            _view.UpdateDescription(data.DescriptionData);
            _view.UpdateWeaponAbility(data.WeaponData);
        }
    }
}


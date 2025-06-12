using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModelViewPresenter
{
    public interface IView
    {
        void UpdateDescription(ItemDescriptionData data);
    }

    public interface IInventoryView : IView { }
    public interface ITrinketView : IView { }
    public interface IWeaponView : IView
    {
        void UpdateWeaponAbility(PlayerWeaponData data);
    }
}


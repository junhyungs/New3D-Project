using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModelViewPresenter
{
    public interface IView { }
    public interface IView<T> : IView
    {
        void UpdateDescription(T data);
    }
    public interface IInventoryView : IView<InventoryItemData> { }
    public interface ITrinketView : IView<TrinketItemData> { }
    public interface IWeaponView : IView<WeaponData>
    {
        void UpdateWeaponAbility(WeaponData data);
    }
}


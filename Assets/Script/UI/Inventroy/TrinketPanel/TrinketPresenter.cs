using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewPresenter;

namespace InventoryUI
{
    public class TrinketPresenter : Presenter<ITrinketView>
    {
        public TrinketPresenter(ITrinketView view) : base(view) { }
    }
}


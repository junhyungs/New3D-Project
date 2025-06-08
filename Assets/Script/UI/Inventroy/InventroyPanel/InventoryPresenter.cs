using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewPresenter;

namespace InventoryUI
{
    public class InventoryPresenter : Presenter<IInventoryView>
    {
        public InventoryPresenter(IInventoryView view) : base(view) { }
    }
}


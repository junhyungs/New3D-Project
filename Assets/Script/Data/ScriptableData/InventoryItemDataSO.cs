using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "InventoryItemDataSO", menuName = "ScriptableObject/Data/InventoryItemDataSO")]
    public class InventoryItemDataSO : ItemDataSO
    {
        public InventoryItemData InventoryItemData => new InventoryItemData(ItemName, ItemDescription);
    }
}


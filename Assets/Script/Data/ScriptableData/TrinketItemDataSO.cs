using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "TrinketItemDataSO", menuName = "ScriptableObject/Data/TrinketItemDataSO")]
    public class TrinketItemDataSO : ItemDataSO
    {
        public TrinketItemData TrinketItemData => new TrinketItemData(ItemName, ItemDescription);
    }
}


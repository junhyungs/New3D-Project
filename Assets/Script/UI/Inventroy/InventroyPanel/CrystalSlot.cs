using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI
{
    public class CrystalSlot : InventorySlot
    {
        [Header("DataKey"), SerializeField]
        private DataKey _key;

        public override ItemDescriptionData DescriptionData
        {
            get => _descriptionData;
            set => _descriptionData = value;
        }

        protected override void Awake()
        {
            base.Awake();

            var data = DataManager.Instance.GetData(_key) as ItemDescriptionData;
            if (data == null)
                return;

            _descriptionData = data;
        }
    }
}


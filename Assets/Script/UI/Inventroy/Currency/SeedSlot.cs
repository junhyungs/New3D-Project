using EnumCollection;
using GameData;

namespace InventoryUI
{
    public class SeedSlot : CurrencyItemSlot
    {
        private void Start()
        {
            _key = UIEvent.SeedView.ToString();
            InventoryManager.Instance.RegisterSlot(ItemType.Seed, this);
        }

        public override int Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                UIManager.TriggerUIEvent(_key, _currency);
            }
        }

        public override void SaveCurrency(PlayerInventoryData inventoryData)
        {
            inventoryData.SeedCount = _currency;
        }
    }
}


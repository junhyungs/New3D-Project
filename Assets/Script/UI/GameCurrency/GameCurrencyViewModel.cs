using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewViewModel;
using EnumCollection;

namespace PlayerInfoUI
{
    public class GameCurrencyViewModel : ViewModel
    {
        private UIEvent _seedKey = UIEvent.SeedView;
        private UIEvent _soulKey = UIEvent.SoulView;

        private int _seedCount;
        public int SeedCount
        {
            get => _seedCount;
            set
            {
                if (_seedCount == value)
                    return;

                _seedCount = value;
                OnPropertyChanged(nameof(SeedCount));
            }
        }

        private int _soulCount;
        public int SoulCount
        {
            get => _soulCount;
            set
            {
                if (_soulCount == value)
                    return;

                _soulCount = value;
                OnPropertyChanged(nameof(SoulCount));
            }
        }

        public void ChangeSeed(int seed)
        {
            SeedCount = seed;
        }

        public void ChangeSoul(int soul)
        {
            SoulCount = soul;
        }

        public override void RegisterEvent()
        {
            var seedKey = _seedKey.ToString();
            var soulKey = _soulKey.ToString();

            UIManager.RegisterUIEvent<int>(seedKey, ChangeSeed);
            UIManager.RegisterUIEvent<int>(soulKey, ChangeSoul);
        }

        public override void UnRegisterEvent()
        {
            var seedKey = _seedKey.ToString();
            var soulKey = _soulKey.ToString();

            UIManager.UnRegisterUIEvent<int>(seedKey, ChangeSeed);
            UIManager.UnRegisterUIEvent<int>(soulKey, ChangeSoul);
        }
    }

}

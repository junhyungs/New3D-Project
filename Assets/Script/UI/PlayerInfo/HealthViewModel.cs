using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewViewModel;
using EnumCollection;

namespace PlayerInfoUI
{
    public class HealthViewModel : ViewModel
    {
        public HealthViewModel()
        {
            _key = UIEvent.HealthView;
        }

        private UIEvent _key;
        private int _health;

        public int Health
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;

                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }

        public void ChangeHealth(int health)
        {
            Health = health;
        }

        public override void RegisterEvent()
        {
            var key = _key.ToString();
            UIManager.RegisterUIEvent<int>(key, ChangeHealth);
        }

        public override void UnRegisterEvent()
        {
            var key = _key.ToString();
            UIManager.UnRegisterUIEvent<int>(key, ChangeHealth);
        }
    }
}


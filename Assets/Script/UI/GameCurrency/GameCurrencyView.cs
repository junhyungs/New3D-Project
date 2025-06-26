using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewViewModel;
using System.ComponentModel;
using UnityEngine.UI;
using TMPro;

namespace PlayerInfoUI
{
    public class GameCurrencyView : View<GameCurrencyViewModel>, IOnAwakeRegisterView
    {
        [Header("ValueText")]
        [SerializeField] private TextMeshProUGUI _seedValueText;
        [SerializeField] private TextMeshProUGUI _soulValueText;

        public void InitializeView()
        {
            Initialize();
        }

        private void OnDisable()
        {
            ClearHandler();
        }

        protected override void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            switch(args.PropertyName)
            {
                case nameof(_viewModel.SeedCount):
                    _seedValueText.text = _viewModel.SeedCount.ToString();
                    break;
                case nameof(_viewModel.SoulCount):
                    _soulValueText.text = _viewModel.SoulCount.ToString();
                    break;
            }
        }
    }
}


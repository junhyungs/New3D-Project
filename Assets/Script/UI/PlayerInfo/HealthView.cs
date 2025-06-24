using ModelViewViewModel;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace PlayerInfoUI
{
    public class HealthView : View<HealthViewModel>, IOnAwakeRegisterView
    {
        [Header("HealthImage"), SerializeField]
        private GameObject[] _activeHealths;
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
            if(args.PropertyName == nameof(_viewModel.Health))
                ChangeHealthImage(_viewModel.Health);
        }

        private void ChangeHealthImage(int health)
        {
            ResetHealthImage();

            for(int i = 0; i < health; i++)
                _activeHealths[i].SetActive(true);
        }

        private void ResetHealthImage()
        {
            foreach (var image in _activeHealths)
                image.SetActive(false);
        }
    }
}


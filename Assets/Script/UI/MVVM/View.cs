using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace ModelViewViewModel
{
    public class View<T> : MonoBehaviour
        where T : ViewModel, new()
    {
        protected T _viewModel;

        protected void Initialize()
        {
            if (_viewModel == null)
            {
                _viewModel = new T();
                _viewModel.PropertyChanged += OnPropertyChangedEvent;
                _viewModel.RegisterEvent();
            }
        }

        protected void Initialize<TParameter>(TParameter parameter)
        {
            _viewModel = new T();
            _viewModel.PropertyChanged += OnPropertyChangedEvent;
            _viewModel.RegisterEvent(parameter);
        }

        protected void ClearHandler()
        {
            if (_viewModel != null)
                _viewModel.UnRegisterEvent();
        }

        protected void ClearHandler<TParameter>(TParameter parameter)
        {
            if (_viewModel != null)
                _viewModel.UnRegisterEvent(parameter);
        }

        protected virtual void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs args) { }
    }
}


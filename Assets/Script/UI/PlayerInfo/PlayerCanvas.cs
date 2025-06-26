using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerInfoUI
{
    public class PlayerCanvas : MonoBehaviour
    {
        [Header("View"), SerializeField]
        private MonoBehaviour[] _viewBehaviours;
        [Header("PlayerUI"), SerializeField]
        private GameObject _playerUI;

        private void Awake()
        {
            foreach (var mono in _viewBehaviours)
            {
                if (mono is IOnAwakeRegisterView view)
                    view.InitializeView();
            }

            var key = EnableUI.PlayerUI.ToString();
            UIManager.Instance.RegisterUI(key, _playerUI);
        }

        private void OnDestroy()
        {
            var key = EnableUI.PlayerUI.ToString();
            UIManager.Instance.UnRegisterUI(key);
        }
    }

    public interface IOnAwakeRegisterView
    {
        void InitializeView();
    }
}


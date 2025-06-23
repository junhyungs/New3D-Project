using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EnumCollection;

namespace PlayerInfoUI
{
    public class PlayerInfo : MonoBehaviour
    {
        [Header("Duration")]
        [SerializeField] private float _moveDuration;
        [SerializeField] private float _fadeDuration;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _startPosition;
        private Vector2 _movePosition;

        private void Awake()
        {
            OnAwakeUIManager();
            Initialize();
        }

        private void OnDestroy()
        {
            OnDestroyUIManager();
        }

        private void Initialize()
        {
            GetComponent();
            SetUIPosition();
        }

        private void OnAwakeUIManager()
        {
            UIManager.PlayerInfoUIController += InfoUIControl;
            var key = EnableUI.PlayerInfoUI.ToString();
            UIManager.Instance.RegisterUI(key, gameObject);
        }

        private void OnDestroyUIManager()
        {
            UIManager.PlayerInfoUIController -= InfoUIControl;
            var key = EnableUI.PlayerInfoUI.ToString();
            UIManager.Instance.UnRegisterUI(key);
        }

        private void GetComponent()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void SetUIPosition()
        {
            _startPosition = _rectTransform.anchoredPosition;
            var targetY = Mathf.Abs(_startPosition.y);
            _movePosition = new Vector2(_startPosition.x, targetY);
        }

        private void InfoUIControl(bool enable)
        {
            if (enable)
                MoveUI(_movePosition, 0f);
            else
                MoveUI(_startPosition, 1f);
        }

        private void MoveUI(Vector2 targetPosition, float targetAlpha)
        {
            _rectTransform.DOAnchorPos(targetPosition, _moveDuration).SetUpdate(true);
            ChangeAlpha(targetAlpha);
        }

        private void ChangeAlpha(float targetAlpha)
        {
            _canvasGroup.DOFade(targetAlpha, _fadeDuration).SetUpdate(true);
        }
    }
}


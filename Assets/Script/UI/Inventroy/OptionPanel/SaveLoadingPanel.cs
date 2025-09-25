using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryUI
{
    public class SaveLoadingPanel : MonoBehaviour
    {
        [Header("Images")]
        [SerializeField] private Image[] _images;
        [Header("RotateSpeed")]
        [SerializeField] private float _speed;
        private WaitForSeconds _loadingTime;
        private Coroutine _loadingCoroutine;

        private Color _changeColor = new Color(0f, 0f, 0f);
        private Color[] _originColor;

        private void Awake()
        {
            Init();
        }

        private void OnDisable()
        {
            ResetColor();
        }

        private void Init()
        {
            _originColor = new Color[_images.Length];
            for(int i = 0; i < _originColor.Length; i++)
                _originColor[i] = _images[i].color;

            _loadingTime = new WaitForSeconds(_speed);
        }

        private void ResetColor()
        {
            for(int i = 0; i < _images.Length; i++)
                _images[i].color = _originColor[i];
        }

        public void StartRotate()
        {
            if(_loadingCoroutine != null)
            {
                StopCoroutine(_loadingCoroutine);
                _loadingCoroutine = null;
            }

            _loadingCoroutine = StartCoroutine(ChangeColor());
        }

        public void StopRotate()
        {
            if (_loadingCoroutine != null)
                StopCoroutine(_loadingCoroutine);

            gameObject.SetActive(false);
        }

        private IEnumerator ChangeColor()
        {
            int index = 0;
            while (true)
            {
                _images[index].color = _changeColor;

                var previousIndex = (index - 1 < 0) ? _images.Length - 1 : index - 1;
                _images[previousIndex].color = _originColor[previousIndex];
                index = (index + 1) % _images.Length;

                yield return _loadingTime;
            }
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MapComponent
{
    public class UIAlpha
    {
        private List<Graphic> _graphics;
        private List<TextMeshProUGUI> _textMeshProUGUI;
       
        public UIAlpha(GameObject root)
        {
            _graphics = new List<Graphic>();
            _textMeshProUGUI = new List<TextMeshProUGUI>();

            foreach(var grapic in root.GetComponentsInChildren<Graphic>())
                if(grapic != null)
                    _graphics.Add(grapic);

            foreach(var ugui in root.GetComponentsInChildren<TextMeshProUGUI>())
                if(ugui != null)
                    _textMeshProUGUI.Add(ugui);
        }

        public void SetAlpha(float alpha)
        {
            foreach(var grapic in _graphics)
            {
                var color = grapic.color;
                color.a = alpha;
                grapic.color = color;
            }

            foreach(var pro in _textMeshProUGUI)
            {
                pro.alpha = alpha;
            }
        }

        public void Clear()
        {
            _graphics.Clear();
            _textMeshProUGUI.Clear();
        }
    }

    public class UITrigger : MapTrigger
    {
        [Header("Setting")]
        [Header("Root")]
        [SerializeField] private GameObject _root;
        [Header("FadeDuration")]
        [SerializeField] private float _fadeDuration;
        [Header("DisplayDuration")]
        [SerializeField] private float _displayDuration;

        private WaitForSeconds _waitForSeconds;

        private const float MAX = 1.0f;
        private const float MIN = 0.0f;

        private void Awake()
        {
            _waitForSeconds = new WaitForSeconds(_displayDuration);
        }

        protected override void Trigger(Collider other)
        {
            StartCoroutine(FadeEffect());
        }

        private IEnumerator FadeEffect()
        {
            var uialpha = new UIAlpha(_root);

            var time = 0f;
            while(time < _fadeDuration)
            {
                time += Time.deltaTime;
                var temp = time / _fadeDuration;
                uialpha.SetAlpha(Mathf.Lerp(MIN, MAX, temp));
                yield return null;
            }
            uialpha.SetAlpha(MAX);

            yield return _waitForSeconds;

            time = 0f;
            while(time < _fadeDuration)
            {
                time += Time.deltaTime;
                var temp = time / _fadeDuration;
                uialpha.SetAlpha(Mathf.Lerp(MAX, MIN, temp));
                yield return null;
            }
            uialpha.SetAlpha(MIN);
            uialpha.Clear();
            uialpha = null;
        }
    }
}


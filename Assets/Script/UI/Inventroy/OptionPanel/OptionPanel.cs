using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryUI
{
    public class OptionPanel : MonoBehaviour
    {
        [Header("ResolutionDropdown"), SerializeField]
        private TMP_Dropdown _dropdown;
        private ScreenResolution _screenResolution;
        private GraphicRaycaster _graphicRaycaster;

        private void Awake()
        {
            _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        }

        private void OnEnable()
        {
            _graphicRaycaster.enabled = true;

            if (_screenResolution != null)
                _dropdown.onValueChanged.AddListener(_screenResolution.ChangeResolution);
        }

        private void OnDisable()
        {
            _graphicRaycaster.enabled = false;

            if (_screenResolution != null)
                _dropdown.onValueChanged.RemoveListener(_screenResolution.ChangeResolution);
        }

        private void Start()
        {
            InitializeDropdown();
        }

        private void InitializeDropdown()
        {
            _screenResolution = new ScreenResolution();

            var resolutions = _screenResolution.Resolutions;
            List<string> stringList = new List<string>();

            foreach (Resolution item in resolutions)
            {
                var width = item.width.ToString();
                var height = item.height.ToString();

                stringList.Add($"{width} X {height}");
            }

            _dropdown.options.Clear();
            _dropdown.AddOptions(stringList);
            ResolutionSetting();

            _dropdown.onValueChanged.AddListener(_screenResolution.ChangeResolution);
        }

        private void ResolutionSetting()
        {
            var currentResolution = _screenResolution.GetCurrentResolution();
            var findText = $"{currentResolution.width} X {currentResolution.height}";

            for (int i = 0; i < _dropdown.options.Count; i++)
            {
                if (_dropdown.options[i].text ==  findText)
                {
                    _dropdown.value = i;
                    _dropdown.RefreshShownValue();
                    break;
                }
            }
        }

        public void Save_MainScene()
        {
            //게임 저장하고 메인 씬을 다시 로드하는 로직.
        }

        public void Save_Exit()
        {
            //게임 저장하는 로직.
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif    
        }
    }
}


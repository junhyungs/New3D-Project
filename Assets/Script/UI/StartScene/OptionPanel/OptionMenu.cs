using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StartSceneUI;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class OptionMenu : MenuUI
{
    [Header("ResolutionDropdown"), SerializeField] private TMP_Dropdown _dropdown;
    private ScreenResolution _screenResolution;

    private void OnEnable()
    {
        OnEnableUI();
    }

    public override void OnEnableUI()
    {
        base.OnEnableUI();
        if (_screenResolution != null)
            _dropdown.onValueChanged.AddListener(_screenResolution.ChangeResolution);
    }

    private void OnDisable()
    {
        OnDisableUI();
    }

    public override void OnDisableUI()
    {
        base.OnDisableUI();
        if (_screenResolution != null)
            _dropdown.onValueChanged.RemoveListener(_screenResolution.ChangeResolution);
    }

    public override void CallBackContext(InputAction.CallbackContext context)
    {
        _uiReference.MainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
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

        foreach(Resolution item in resolutions)
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

        for(int i = 0; i < _dropdown.options.Count; i++)
        {
            if (_dropdown.options[i].text ==  findText)
            {
                _dropdown.value = i;
                _dropdown.RefreshShownValue();
                break;
            }
        }
    }
}

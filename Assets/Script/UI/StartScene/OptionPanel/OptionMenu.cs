using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StartSceneUI;
using UnityEngine.InputSystem;

public class OptionMenu : MenuUI
{
    private ScreenResolution _screenResolution;

    private void Start()
    {
        _screenResolution = new ScreenResolution();
    }

    private void OnEnable()
    {
        OnEnableUI();
    }

    private void OnDisable()
    {
        OnDisableUI();
    }

    public override void CallBackContext(InputAction.CallbackContext context)
    {
        _uiReference.MainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}

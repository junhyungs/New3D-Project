using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using StartSceneUI;
using GameData;

public class GameMenu : MenuUI
{
    [Header("SaveInfo"), SerializeField] private SaveInfo[] _saveInfos;

    public int SaveInfoLength => _saveInfos.Length;

    public void SetSaveInfo(PlayerSaveData[] playerSaveDatas)
    {
        for(int i = 0; i < _saveInfos.Length; i++)
        {
            _saveInfos[i].SetInfo(playerSaveDatas[i]);
        }
    }

    private void OnEnable()
    {
        OnEnableUI();
    }

    private void OnDisable()
    {
        OnDisableUI();  
    }

    public override void OnEnableUI()
    {
        base.OnEnableUI();

        EventSystem.current.SetSelectedGameObject(_saveInfos[0].gameObject);
    }

    public override void OnDisableUI()
    {
        base.OnDisableUI();
    }

    public override void CallBackContext(InputAction.CallbackContext context)
    {
        _uiReference.MainMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

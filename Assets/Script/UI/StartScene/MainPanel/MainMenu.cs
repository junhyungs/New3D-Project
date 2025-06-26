using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using StartSceneUI;
using UnityEngine.UI;

public class MainMenu : MenuUI
{
    [Header("Menu"), SerializeField] private Menu[] _menus;

    private Dictionary<GameObject, Menu> _menuDictionary;
    private Button[] _menuButtons;
    private GameObject _currentUI;
    private bool _enableButtons;

    protected override void Awake()
    {
        base.Awake();
        OnAwakeMainMenu();
    }

    private void OnAwakeMainMenu()
    {
        _menuDictionary = new Dictionary<GameObject, Menu>();
        _menuButtons = new Button[_menus.Length];

        for(int i = 0; i < _menus.Length; i++)
        {
            var buttonComponent = _menus[i].GetComponent<Button>();
            if(buttonComponent != null)
            {
                _menuButtons[i] = buttonComponent;
            }
            _menuDictionary.Add(_menus[i].gameObject, _menus[i]);
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
        StartCoroutine(WaitForEndOfFrame());
    }

    private IEnumerator WaitForEndOfFrame() //Canvas 초기화가 완료 되도록 대기하는 코루틴.
    {
        yield return new WaitForEndOfFrame();
        SetSelectedGameObject();
    }

    private void SetSelectedGameObject()
    {
        EventSystem.current.SetSelectedGameObject(_menus[0].gameObject);
        _currentUI = _menus[0].gameObject;

        _menus[0].SelectedMenu();
    }

    public override void OnDisableUI()
    {
        base.OnDisableUI();
    }

    public override void CallBackContext(InputAction.CallbackContext context)
    {
        if (!_enableButtons)
            return;

        Menu menu = GetCurrentMenu(_currentUI);
        menu?.DeSelectMenu();

        StartCoroutine(WaitForUI());
    }

    private IEnumerator WaitForUI() //UI 오브젝트가 갱신되도록 1프레임 기다리는 코루틴.
    {
        yield return null;

        var currentMenuUI = EventSystem.current.currentSelectedGameObject;
        _currentUI = currentMenuUI;

        Menu menu = GetCurrentMenu(currentMenuUI);
        menu?.SelectedMenu();
    }

    private Menu GetCurrentMenu(GameObject currentMenuUI)
    {
        if(_menuDictionary.TryGetValue(currentMenuUI, out Menu menu))
            return menu;

        return null;
    }

    public void EnableButtons(bool enabled)
    {
        _enableButtons = enabled;

        foreach(var button in _menuButtons)
            button.enabled = enabled;
    }

    public void OnClickStartButton()
    {
        PressButton();
    }

    public void OnClickOptionButton()
    {
        PressButton();
    }

    public void OnClickExitButton()
    {
        PressButton();
    }

    private void PressButton()
    {
        var currentMenuUI = EventSystem.current.currentSelectedGameObject;

        Menu menu = GetCurrentMenu(currentMenuUI);
        menu?.DeSelectMenu();
        menu?.PressedMenu(this.gameObject);
    }
}

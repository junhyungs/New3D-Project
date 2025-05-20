using GameData;
using StartSceneUI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameMenu : MenuUI
{
    [Header("SaveInfoView"), SerializeField] private SaveInfoView[] _saveInfos;

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

    private async void Start()
    {
        await InitializeGameMenuAsync();
    }

    private async Task InitializeGameMenuAsync()
    {
        var loadCount = _saveInfos.Length;
        var task = new Task<PlayerSaveData>[loadCount];

        for (int i = 0; i < loadCount; i++)
            task[i] = SaveManager.Instance.LoadPlayerSaveDataAsync(i);

        PlayerSaveData[] savePlayerData = await Task.WhenAll(task);

        for (int i = 0; i < loadCount; i++)
            UIManager.Instance.TriggerUIEvent(_saveInfos[i].EventKey, savePlayerData[i]);
    }

    public override void CallBackContext(InputAction.CallbackContext context)
    {
        _uiReference.MainMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

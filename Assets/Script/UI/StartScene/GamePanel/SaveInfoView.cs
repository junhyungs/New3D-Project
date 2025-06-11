using TMPro;
using UnityEngine;
using ModelViewViewModel;
using System.ComponentModel;
using System;
using EnumCollection;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class SaveInfoView : View<SaveInfoViewModel>
{
    private Action _onClick;
    public string EventKey => _eventKey.ToString();
    
    [Header("SaveIndex"), SerializeField] private int _saveIndex;
    [Header("SaveInfoSlot"), SerializeField] private UIEvent _eventKey;
    [Header("DateText"), SerializeField] private TextMeshProUGUI _dateText;
    [Header("LoadingCanvas"), SerializeField] private LoadingController _loadingController;

    private void OnEnable()
    {
        Initialize(_eventKey);
    }

    private void OnDisable()
    {
        ClearHandler(_eventKey);
    }

    protected override void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs args)
    {
        _dateText.text = _viewModel.PlayerSaveData == null ? "새로운 게임" : _viewModel.PlayerSaveData.Date;
        //OneTimeAction(() => DataManager.Instance.AddToPlayerData(_viewModel.PlayerSaveData));
    }

    public async void OnClickStartGame()
    {
        SaveManager.SaveIndex = _saveIndex;
        //_onClick?.Invoke();
        await _loadingController.LoadingSceneGameDataAsync("MainScene", "GamePanel");
    }

    private void OneTimeAction(Action addToPlayerData)
    {
        Action oneTimeAction = null;
        oneTimeAction = () =>
        {
            addToPlayerData?.Invoke();
            _onClick -= oneTimeAction;
        };
        _onClick += oneTimeAction;
    }
}

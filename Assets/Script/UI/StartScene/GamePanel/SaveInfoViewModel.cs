using EnumCollection;
using GameData;
using ModelViewViewModel;
public class SaveInfoViewModel : ViewModel
{
    private PlayerSaveData _playerSaveData;

    public PlayerSaveData PlayerSaveData
    {
        get => _playerSaveData;
        set
        {
            _playerSaveData = value;
            OnPropertyChanged(nameof(PlayerSaveData));
        }
    }

    public void SetPlayerSaveData(PlayerSaveData playerSaveData)
    {
        PlayerSaveData = playerSaveData;
    }

    public override void RegisterEvent<TParameter>(TParameter parameter)
    {
        var key = parameter.ToString();
        UIManager.Instance.RegisterUIEvent<PlayerSaveData>(key, SetPlayerSaveData);
    }

    public override void UnRegisterEvent<TParameter>(TParameter parameter)
    {
        var key = parameter.ToString();
        UIManager.Instance.UnRegisterUIEvent<PlayerSaveData>(key, SetPlayerSaveData);
    }
}



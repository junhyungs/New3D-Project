using UnityEngine;
using TMPro;
using GameData;
using System;

public class SaveInfo : MonoBehaviour
{
    [Header("DataInfoText"), SerializeField] private TextMeshProUGUI _datainfoText;
    [Header("SaveIndex"), SerializeField] private int _saveIndex;

    private Action _onClick;

    public void SetInfo(PlayerSaveData playerSaveData)
    {
        _datainfoText.text = playerSaveData == null? "새로운 게임" : playerSaveData.Date;
        OneTimeAction(() => DataManager.Instance.AddToPlayerData(playerSaveData));
    }

    public void OnClickStartGame()
    {
        SaveManager.SaveIndex = _saveIndex;
        _onClick.Invoke();
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

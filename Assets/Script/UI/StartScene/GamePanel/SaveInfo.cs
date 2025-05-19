using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameData;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class SaveInfo : MonoBehaviour
{
    [Header("DataInfoText"), SerializeField] private TextMeshProUGUI _datainfoText;

    private Action _onClick;

    public void SetInfo(PlayerSaveData playerSaveData)
    {
        _datainfoText.text = playerSaveData == null? "새로운 게임" : playerSaveData.Date;
        OneTimeAction(() => DataManager.Instance.AddToPlayerData(playerSaveData));
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

    public void OnClickStartGame()
    {
        _onClick.Invoke();
    }
}

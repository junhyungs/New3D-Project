using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameData;
using UnityEngine.UI;

public class SaveInfo : MonoBehaviour
{
    [Header("DataInfoText"), SerializeField] private TextMeshProUGUI _datainfoText;

    public void SetInfo(PlayerSaveData playerSaveData)
    {
        _datainfoText.text = playerSaveData == null? "새로운 게임" : playerSaveData.Date;
    }
}

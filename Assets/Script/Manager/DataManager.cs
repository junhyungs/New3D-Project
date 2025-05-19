using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using System.Threading.Tasks;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, Data> _dataDictionary = new Dictionary<string, Data>();
  
    private void TryAddData(string key, Data data)
    {
        if (_dataDictionary.TryAdd(key, data)) Debug.Log("AddData");
        else Debug.Log("Fail");
    }

    public void AddToPlayerData(PlayerSaveData playerSaveData)
    {
        if(playerSaveData == null)
        {
            //TODO
            return;
        }

        var key = playerSaveData.Id;
        TryAddData(key, playerSaveData);
    }
}



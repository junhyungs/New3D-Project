using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using System.Threading.Tasks;
using EnumCollection;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, Data> _dataDictionary = new Dictionary<string, Data>();

    private void TryAddData(string key, Data data)
    {
        if (_dataDictionary.TryAdd(key, data)) Debug.Log("AddData");
        else Debug.Log("Fail");
    }

    public Data GetData(string key)
    {
        if(_dataDictionary.TryGetValue(key, out Data data))
            return data;

        Debug.Log("Data Null");
        return null;
    }

    #region Player
    public void AddToPlayerData(PlayerSaveData playerSaveData)
    {
        if (playerSaveData == null)
        {
            var jsonDataName = JsonData.New_3D_Player.ToString();

            try
            {
                var jsonData = Resources.Load<TextAsset>($"JsonData/{jsonDataName}");
                ParsePlayerBaseData(jsonData.text);
            }
            catch
            {
                Debug.Log("AddToPlayerData");
            }

            return;
        }

        var key = playerSaveData.Id;
        TryAddData(key, playerSaveData);
    }

    private void ParsePlayerBaseData(string jsonData) //새로운 게임인 경우 호출.
    {
        //List<PlayerSaveData> dataList = 
        //    JsonConvert.DeserializeObject<List<PlayerSaveData>>(jsonData); List

        JArray jsonArray = JArray.Parse(jsonData);
       
        var item = jsonArray[0];

        string id = ParseString(item["Id"]);
        int power = ParseInt(item["Power"]);
        float speed = ParseFloat(item["Speed"]);
        float rollSpeed = ParseFloat(item["RollSpeed"]);
        float ladder = ParseFloat(item["LadderSpeed"]);
        float changeValue = ParseFloat(item["SpeedChangeValue"]);
        float speedOffSet = ParseFloat(item["SpeedOffSet"]);
        int health = ParseInt(item["Health"]);

        PlayerSaveData data = new PlayerSaveData();
        data.SetPlayerData(id, power, speed, rollSpeed, ladder,
            changeValue, speedOffSet, health);

        SaveManager.Instance.SavePlayerData(data);
        TryAddData(id, data);
    }
    #endregion

    private string ParseString(JToken jToken)
    {
        return jToken != null ? jToken.ToString() : string.Empty;
    }

    private float ParseFloat(JToken jToken)
    {
        if (jToken == null || !float.TryParse(jToken.ToString(), out float value))
            return 0f;

        return value;
    }

    private int ParseInt(JToken jToken)
    {
        if(jToken == null || !int.TryParse(jToken.ToString(), out int value))
            return 0;

        return value;
    }

}



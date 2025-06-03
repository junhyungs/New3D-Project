using EnumCollection;
using GameData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, Data> _dataDictionary = new Dictionary<string, Data>();

    private void TryAddData(string key, Data data)
    {
        if (_dataDictionary.TryAdd(key, data)) Debug.Log("AddData");
        else Debug.Log("Fail");
    }

    public Data GetData(DataKey key)
    {
        var stringKey = key.ToString();
        if(_dataDictionary.TryGetValue(stringKey, out Data data))
            return data;

        Debug.Log("Data Null");
        return null;
    }

    public async Task LoadAllData()
    {
        var taskList = new List<Task>()
        {
            LoadPathData(),
        };
        
        await Task.WhenAll(taskList);
    }

    public async Task<JArray> LoadJsonArrayAsync(string path)
    {
        var request = Resources.LoadAsync<TextAsset>(path);

        while (!request.isDone)
            await Task.Yield();

        var textAsset = request.asset as TextAsset;
        return JArray.Parse(textAsset.text);
    }

    public JArray LoadJsonArray(string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        return JArray.Parse(textAsset.text);
    }

    #region PathData
    public async Task LoadPathData()
    {
        JArray jArray = await LoadJsonArrayAsync($"JsonData/{JsonData.New_3D_Path}");

        foreach(var item in jArray)
        {
            string id = ParseString(item);
            string path = ParseString(item);

            PathData pathData = new PathData(id, path);
            TryAddData(id, pathData);
        }
    }

    public void TestLoadPathData()
    {
        try
        {
            var textAsset = Resources.Load<TextAsset>("JsonData/New_3D_Path");
            JArray jArray = JArray.Parse(textAsset.text);

            foreach (var item in jArray)
            {
                string id = ParseString(item["ID"]);
                string path = ParseString(item["Path"]);

                PathData pathData = new PathData(id, path);
                TryAddData(id, pathData);
            }
        }
        catch { }
    }

    #endregion
    #region Player
    public void AddToPlayerData(PlayerSaveData playerSaveData)
    {
        if (playerSaveData == null)
        {
            try
            {
                var jsonDataName = JsonData.New_3D_Player.ToString();
                ParsePlayerBaseData($"JsonData/{jsonDataName}");
            }
            catch
            {
                Debug.Log("AddToPlayerData");
            }

            return;
        }

        var key = playerSaveData.ID;
        TryAddData(key, playerSaveData);
    }

    private void ParsePlayerBaseData(string path) //새로운 게임인 경우 호출.
    {
        JArray jsonArray = LoadJsonArray(path);  
        var item = jsonArray[0];

        string id = ParseString(item["ID"]);
        int power = ParseInt(item["Power"]);
        float speed = ParseFloat(item["Speed"]);
        float rollSpeed = ParseFloat(item["RollSpeed"]);
        float ladder = ParseFloat(item["LadderSpeed"]);
        float changeValue = ParseFloat(item["SpeedChangeValue"]);
        float speedOffSet = ParseFloat(item["SpeedOffSet"]);
        float dashSpeed = ParseFloat(item["DashSpeed"]);
        int health = ParseInt(item["Health"]);

        PlayerConstantData constantData = new PlayerConstantData(
            rollSpeed, ladder, changeValue, speedOffSet, dashSpeed);

        PlayerSaveData data = new PlayerSaveData()
        {            
            ID = id,
            Power = power,
            Speed = speed,
            Health = health,
            ConstantData = constantData,
            SkillDictionary = new Dictionary<string, PlayerSkillData>()
        };

        ParsePlayerSkillData(data);
        //SaveManager.Instance.SavePlayerData(data); 주석 마지막에 풀어줘야함.
        TryAddData(id, data);
    }

    private void ParsePlayerSkillData(PlayerSaveData saveData)
    {
        var skillDictionary = saveData.SkillDictionary;

        var path = $"JsonData/{JsonData.New_3D_PlayerSkill}";
        JArray jArray = LoadJsonArray(path);

        foreach(var item in jArray)
        {
            string id = ParseString(item["ID"]);
            float projectileSpeed = ParseFloat(item["ProjectileSpeed"]);
            float flightTime = ParseFloat(item["FlightTime"]);
            int projectileDamage = ParseInt(item["ProjectileDamage"]);
            int projectileCost = ParseInt(item["ProjectileCost"]);

            PlayerSkillData data = new PlayerSkillData(
                id, projectileSpeed, projectileDamage, projectileCost, flightTime);

            skillDictionary.Add(id, data);
        }
    }
    #endregion
    #region Resolution
    public List<(int, int)> LoadResolutionData()
    {
        var dataList = new List<(int, int)>();
        var jsonName = JsonData.New_3D_ScreenResolution.ToString();
        var jsonData = Resources.Load<TextAsset>($"JsonData/{jsonName}");

        JArray jArray = JArray.Parse(jsonData.text);
        foreach (var itme in jArray)
        {
            int width = ParseInt(itme["Width"]);
            int height = ParseInt(itme["Height"]);

            dataList.Add((width, height));
        }

        return dataList;
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



using EnumCollection;
using GameData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, Data> _dataDictionary = new Dictionary<string, Data>();

    private void TryAddData(string key, Data data)
    {
        if (_dataDictionary.TryAdd(key, data)) Debug.Log($"AddData : {key}");
        else Debug.Log($"Fail : {key}");
    }

    public Data GetData(string key)
    {
        if(_dataDictionary.TryGetValue(key, out Data data))
            return data;

        Debug.Log("Data Null");
        return null;
    }

    public async UniTask LoadAllData()
    {
        var taskList = new List<UniTask>()
        {
            AddToPlayerDataAsync(),
            LoadPathData(),
            LoadItemDescriptionData(),
        };
        
        await UniTask.WhenAll(taskList);
    }

    private async UniTask<JArray> LoadJsonArrayAsync(string path)
    {
        var request = Resources.LoadAsync<TextAsset>(path);
        await request.ToUniTask();

        var textAsset = request.asset as TextAsset;
        return JArray.Parse(textAsset.text);
    }

    private JArray LoadJsonArray(string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        return JArray.Parse(textAsset.text);
    }

    #region ItemDescriptionData
    private async UniTask LoadItemDescriptionData()
    {
        JArray jArray = await LoadJsonArrayAsync($"JsonData/New_3D_ItemDescription");
        foreach (var item in jArray)
        {
            string id = ParseString(item["Id"]);
            string itemName = ParseString(item["ItemName"]);
            string description = ParseString(item["Description"]);

            ItemDescriptionData data = new ItemDescriptionData(id, itemName, description);
            TryAddData(id, data);
        }
    }

    public void TestLoadItemDescriptionData()
    {
        try
        {
            var textAsset = Resources.Load<TextAsset>("JsonData/New_3D_ItemDescription");
            JArray jArray = JArray.Parse(textAsset.text);
            foreach (var item in jArray)
            {
                string id = ParseString(item["Id"]);
                string itemName = ParseString(item["ItemName"]);
                string description = ParseString(item["Description"]);

                ItemDescriptionData data = new ItemDescriptionData(id, itemName, description);
                TryAddData(id, data);
            }
        }
        catch(Exception e)
        {
            Debug.Log("TestLoadItemDescriptionData");
            Debug.Log(e.Message);
        }
    }

    #endregion
    #region PathData
    private async UniTask LoadPathData()
    {
        JArray jArray = await LoadJsonArrayAsync($"JsonData/New_3D_Path");

        foreach(var item in jArray)
        {
            string id = ParseString(item["ID"]);
            string path = ParseString(item["Path"]);

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
    public async UniTask AddToPlayerDataAsync()
    {
        var index = SaveManager.SaveIndex;
        PlayerSaveData saveData = await SaveManager.Instance.LoadPlayerSaveDataAsync(index);

        if(saveData != null)
        {
            var key = saveData.ID;
            TryAddData(key, saveData);
        }
        else
        {
            try
            {
                await NewPlayerDataAsync();
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

    private async UniTask NewPlayerDataAsync()
    {
        List<UniTask> taskList = new List<UniTask>()
        {
            ParsePlayerBaseDataAsync(),
            ParsePlayerSkillDataAsync()
        };

        await UniTask.WhenAll(taskList);
    }

    private async UniTask ParsePlayerBaseDataAsync()
    {
        var path = $"JsonData/New_3D_Player";
        JArray jArray = await LoadJsonArrayAsync(path);
        var item = jArray[0];

        var newSaveData = new PlayerSaveData();
        newSaveData.ID = ParseString(item["ID"]);
        newSaveData.Power = ParseInt(item["Power"]);
        newSaveData.Speed = ParseFloat(item["Speed"]);
        newSaveData.Health = ParseInt(item["Health"]);
        newSaveData.ConstantData = ParseConstantData(item);

        TryAddData(newSaveData.ID, newSaveData);
    }

    private PlayerConstantData ParseConstantData(JToken item)
    {
        var rollSpeed = ParseFloat(item["RollSpeed"]);
        var ladder = ParseFloat(item["LadderSpeed"]);
        var changeValue = ParseFloat(item["SpeedChangeValue"]);
        var speedOffSet = ParseFloat(item["SpeedOffSet"]);
        var dashSpeed = ParseFloat(item["DashSpeed"]);

        PlayerConstantData constantData = new PlayerConstantData(rollSpeed,
            ladder, changeValue, speedOffSet, dashSpeed);

        return constantData;
    }

    private async UniTask ParsePlayerSkillDataAsync()
    {
        var path = $"JsonData/New_3D_PlayerSkill";
        JArray jArray = await LoadJsonArrayAsync(path);

        foreach(var item in jArray)
        {
            var id = ParseString(item["ID"]);
            var projectileSpeed = ParseFloat(item["ProjectileSpeed"]);
            var flightTime = ParseFloat(item["FlightTime"]);
            var projectileDamage = ParseInt(item["ProjectileDamage"]);
            var projectileCost = ParseInt(item["ProjectileCost"]);

            PlayerSkillData data = new PlayerSkillData(id, projectileSpeed, projectileDamage,
                projectileCost, flightTime);

            TryAddData(id, data);
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
    #region TestCode
    public void AddToPlayerData(PlayerSaveData playerSaveData)
    {
        if (playerSaveData == null)
        {
            try
            {
                var jsonDataName = JsonData.New_3D_Player.ToString();
                ParsePlayerBaseData($"JsonData/{jsonDataName}");
                ParsePlayerSkillData();
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
            ConstantData = constantData
        };

        //SaveManager.Instance.SavePlayerData(data); 주석 마지막에 풀어줘야함.
        TryAddData(id, data);
    }

    private void ParsePlayerSkillData()
    {
        var path = $"JsonData/{JsonData.New_3D_PlayerSkill}";
        JArray jArray = LoadJsonArray(path);

        foreach (var item in jArray)
        {
            string id = ParseString(item["ID"]);
            float projectileSpeed = ParseFloat(item["ProjectileSpeed"]);
            float flightTime = ParseFloat(item["FlightTime"]);
            int projectileDamage = ParseInt(item["ProjectileDamage"]);
            int projectileCost = ParseInt(item["ProjectileCost"]);

            PlayerSkillData data = new PlayerSkillData(
                id, projectileSpeed, projectileDamage, projectileCost, flightTime);

            TryAddData(id, data);
        }
    }

    private void ParsePlayerWeaponData()
    {

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



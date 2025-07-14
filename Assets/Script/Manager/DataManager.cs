using EnumCollection;
using GameData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, Data> _dataDictionary = new Dictionary<string, Data>();
    private Dictionary<ScriptableDataKey, ScriptableData> _scriptableDataDictionary
        = new Dictionary<ScriptableDataKey, ScriptableData>();

    private bool TryAddData(string key, Data data)
    {
        Debug.Log($"TryAddData : {key}");
        if (_dataDictionary.TryAdd(key, data))
        {
            Debug.Log($"AddData : {key}");
            return true;
        }
        else
        {
            Debug.Log($"Fail : {key}");
            return false;
        }
    }

    private bool TryAddScriptableData(ScriptableDataKey key, ScriptableData data)
    {
        Debug.Log($"TryAddScriptableData : {key}");
        if (_scriptableDataDictionary.TryAdd(key, data))
        {
            Debug.Log($"Add : {key}");
            return true;
        }
        else
        {
            Debug.Log($"Fail : {key}");
            return false;
        }
    }

    public Data GetData(string key)
    {
        if(_dataDictionary.TryGetValue(key, out Data data))
            return data;

        Debug.Log("Data Null");
        return null;
    }

    public ScriptableData GetScriptableData(ScriptableDataKey key)
    {
        if(_scriptableDataDictionary.TryGetValue(key,out ScriptableData data))
            return data;

        Debug.Log("Data Null");
        return null;
    }

    public async UniTask LoadAllData()
    {
        await LoadPlayerGroup();
        await LoadUpgradeGroup();
        await LoadMapData();
        await LoadAllScriptableDataAsync();
    }

    private async UniTask LoadPlayerGroup()
    {
        await UniTask.WhenAll(
            ParsePlayerBaseDataAsync(),
            LoadPlayerInventoryDataAsync()
            );
    }

    private async UniTask LoadUpgradeGroup()
    {
        await UniTask.WhenAll(
            LoadUpgradeAbilityDataAsync(),
            LoadUpgradeSkillDataAsync()
            );
    }
 
    private async UniTask<JArray> LoadJsonArrayAsync(string address)
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(address);
        var textAsset = await handle.ToUniTask();

        var json = JArray.Parse(textAsset.text);
        Addressables.Release(handle);
        return json;
    }

    private async UniTask LoadAllScriptableDataAsync()
    {
        var label = AddressablesKey._scriptableDataLabels;

        var handle = Addressables.LoadAssetsAsync<ScriptableData>(label, null);
        var result = await handle.ToUniTask();
        foreach(var data in result)
        {
            var key = data.Key;
            TryAddScriptableData(key, data);
        }

        Addressables.Release(handle);
    }

    private async UniTask ParsePlayerBaseDataAsync()
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
                var address = AddressablesKey.JsonData_PlayerBase;
                JArray jArray = await LoadJsonArrayAsync(address);
                var item = jArray[0];

                var newSaveData = new PlayerSaveData();
                newSaveData.ID = ParseString(item["ID"]);
                newSaveData.Power = ParseInt(item["Power"]);
                newSaveData.Speed = ParseFloat(item["Speed"]);
                newSaveData.Health = ParseInt(item["Health"]);
                newSaveData.ConstantData = ParseConstantData(item);

                if (TryAddData(newSaveData.ID, newSaveData))
                {
                    var directoryPath = SaveManager.DirectoryPath;
                    await SaveManager.Instance.SavePlayer(directoryPath);
                }
                else
                    throw new Exception("Player TryAddData Fail");
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
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

    private async UniTask LoadPlayerInventoryDataAsync()
    {
        var key = DataKey.Inventory_Data.ToString();
        PlayerInventoryData data = await SaveManager.Instance.LoadPlayerInventoryDataAsync();
        if (data != null)
            TryAddData(key, data);
        else
        {
            data = new PlayerInventoryData();
            TryAddData(key, data);
        }
    }

    private async UniTask LoadUpgradeSkillDataAsync()
    {
        var key = DataKey.SkillUpgrade.ToString();
        PlayerUpgradeData_Skill data = await SaveManager.Instance.LoadPlayerUpgradeData_SkillAsync();
        if(data != null)
            TryAddData(key, data);
        else
        {
            data = new PlayerUpgradeData_Skill(0);
            TryAddData(key, data);
        }
    }

    private async UniTask LoadUpgradeAbilityDataAsync()
    {
        var key = DataKey.AbilityUpgrade.ToString();
        PlayerUpgradeData_Ability data = await SaveManager.Instance.LoadPlayerUpgradeData_AbilityAsync();
        if(data != null)
            TryAddData(key, data);
        else
        {
            data = new PlayerUpgradeData_Ability(0, 0f);
            TryAddData(key, data);
        }
    }

    private async UniTask LoadMapData()
    {
        var key = DataKey.Map_Data.ToString();
        var mapData = await SaveManager.Instance.LoadMapSaveDataAsync();
        if (mapData != null)
            TryAddData(key, mapData);
        else
        {
            var progressDic = new Dictionary<string, MapProgress>();
            mapData = new MapData(progressDic);
            TryAddData(key, mapData);
        }
    }

    public List<(int, int)> LoadResolutionData()
    {
        var dataList = new List<(int, int)>();
        var jsonName = AddressablesKey.JsonData_ScreenResolution;
        var jsonData = Resources.Load<TextAsset>(jsonName);

        JArray jArray = JArray.Parse(jsonData.text);
        foreach (var itme in jArray)
        {
            int width = ParseInt(itme["Width"]);
            int height = ParseInt(itme["Height"]);

            dataList.Add((width, height));
        }

        return dataList;
    }

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

    private Vector3 ParseVector3(JToken jToken)
    {
        var stringValue = ParseString(jToken);
        var trim = stringValue.Trim('{', '}');
        var splitArray = trim.Split(',');

        var x = ParseFloat(splitArray[0]);
        var y = ParseFloat(splitArray[1]);
        var z = ParseFloat(splitArray[2]);

        return new Vector3(x, y, z);
    }
}



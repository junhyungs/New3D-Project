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

    public Data GetData(string key)
    {
        if(_dataDictionary.TryGetValue(key, out Data data))
            return data;

        Debug.Log("Data Null");
        return null;
    }

    #region LoadAllData
    public async UniTask LoadAllData()
    {
        await LoadPlayerGroup();
        await LoadItemDescriptionData();
        await LoadDialogData();
        await LoadUpgradeGroup();
        await LoadMapData();
    }

    private async UniTask LoadPlayerGroup()
    {
        await ParsePlayerBaseDataAsync();

        await UniTask.WhenAll(
            ParsePlayerSkillDataAsync(),
            ParsePlayerWeaponDataAsync(),
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
    #endregion
    private async UniTask<JArray> LoadJsonArrayAsync(string address)
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(address);
        var textAsset = await handle.ToUniTask();

        var json = JArray.Parse(textAsset.text);
        Addressables.Release(handle);
        return json;
    }

    private JArray LoadJsonArray(string path) //최종 빌드 시 삭제
    {
        var textAsset = Resources.Load<TextAsset>(path);
        return JArray.Parse(textAsset.text);
    }

    #region DialogData
    private async UniTask LoadDialogData()
    {
        var address = AddressablesKey.JsonData_Dialog;
        JArray jArray = await LoadJsonArrayAsync(address);
        var dialogDictionary = new Dictionary<string, Dialog>();
        try
        {
            foreach (var item in jArray)
            {
                var id = ParseString(item["Id"]);
                var name = ParseString(item["Name"]);
                var storyList = ParseDialogList(item["Story"]);
                var loopList = ParseDialogList(item["Loop"]);
                var endList = ParseDialogList(item["End"]);

                Dialog dialog = new Dialog(name, storyList, loopList, endList);
                dialogDictionary.Add(id, dialog);
            }

            DialogData data = new DialogData(dialogDictionary);
            var key = DataKey.DialogData.ToString();
            TryAddData(key, data);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private List<string> ParseDialogList(JToken jToken)
    {
        var message = jToken.ToString();
        var splitArray = message.Split("{E}");
        var dialogList = splitArray.ToList();
        return dialogList;
    }
    #endregion
    #region ItemDescriptionData
    private async UniTask LoadItemDescriptionData()
    {
        var address = AddressablesKey.JsonData_ItemDescription;
        JArray jArray = await LoadJsonArrayAsync(address);
        try
        {
            foreach (var item in jArray)
            {
                string id = ParseString(item["Id"]);
                string itemName = ParseString(item["ItemName"]);
                string description = ParseString(item["Description"]);

                ItemDescriptionData data = new ItemDescriptionData(id, itemName, description);
                TryAddData(id, data);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion
    #region Player
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

    private async UniTask ParsePlayerSkillDataAsync()
    {
        var address = AddressablesKey.JsonData_PlayerSkill;
        JArray jArray = await LoadJsonArrayAsync(address);
        
        try
        {
            foreach (var item in jArray)
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
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private async UniTask ParsePlayerWeaponDataAsync()
    {
        var address = AddressablesKey.JsonData_PlayerWeapon;
        JArray jArray = await LoadJsonArrayAsync(address);

        try
        {
            foreach (var item in jArray)
            {
                var id = ParseString(item["Id"]);
                var damage = ParseInt(item["Damage"]);
                var range = ParseVector3(item["Range"]);

                PlayerWeaponData data = new PlayerWeaponData(id, damage, range);
                TryAddData(id, data);
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
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
    #endregion
    #region Resolution
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
    #endregion
    #region Map
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
    #endregion
    #region TestCode
    public void TestLoadDialogData()
    {
        try
        {
            var dialogDictionary = new Dictionary<string, Dialog>();
            JArray jArray = LoadJsonArray("JsonData/New_3D_Dialog");
            foreach (var item in jArray)
            {
                var id = ParseString(item["Id"]);
                var name = ParseString(item["Name"]);
                var storyList = ParseDialogList(item["Story"]);
                var loopList = ParseDialogList(item["Loop"]);
                var endList = ParseDialogList(item["End"]);

                Dialog dialog = new Dialog(name, storyList, loopList, endList);
                dialogDictionary.Add(id, dialog);
            }

            DialogData data = new DialogData(dialogDictionary);
            var key = DataKey.DialogData.ToString();
            TryAddData(key, data);
        }
        catch { }
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
        catch (Exception e)
        {
            Debug.Log("TestLoadItemDescriptionData");
            Debug.Log(e.Message);
        }
    }

    public void AddToPlayerData(PlayerSaveData playerSaveData)
    {
        if (playerSaveData == null)
        {
            try
            {
                var jsonDataName = AddressablesKey.JsonData_PlayerBase;
                ParsePlayerBaseData(jsonDataName);
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
        var path = AddressablesKey.JsonData_PlayerSkill;
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

    public void ParsePlayerWeaponData()
    {
        var path = AddressablesKey.JsonData_PlayerWeapon;
        JArray jArray = LoadJsonArray(path);

        foreach (var item in jArray)
        {
            var id = ParseString(item["Id"]);
            var damage = ParseInt(item["Damage"]);
            var range = ParseVector3(item["Range"]);

            PlayerWeaponData data = new PlayerWeaponData(id, damage, range);
            TryAddData(id, data);
        }
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



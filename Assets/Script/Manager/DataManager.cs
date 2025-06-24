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
using System.Linq;

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

    public async UniTask LoadAllData()
    {
        await LoadPlayerGroup();
        await LoadItemGroup();
        await LoadDialogData();
        await LoadUpgradeGroup();
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

    private async UniTask LoadItemGroup()
    {
        await UniTask.WhenAll(
            LoadPathData(),
            LoadItemDescriptionData()
            );
    }

    private async UniTask LoadUpgradeGroup()
    {
        await UniTask.WhenAll(
            LoadUpgradeAbilityDataAsync(),
            LoadUpgradeSkillDataAsync()
            );
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

    #region DialogData
    private async UniTask LoadDialogData()
    {
        JArray jArray = await LoadJsonArrayAsync("JsonData/New_3D_Dialog");
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
        JArray jArray = await LoadJsonArrayAsync("JsonData/New_3D_ItemDescription");

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
    #region PathData
    private async UniTask LoadPathData()
    {
        JArray jArray = await LoadJsonArrayAsync($"JsonData/New_3D_Path");

        try
        {
            foreach (var item in jArray)
            {
                string id = ParseString(item["ID"]);
                string path = ParseString(item["Path"]);

                PathData pathData = new PathData(id, path);
                TryAddData(id, pathData);
            }
        }
        catch(Exception ex)
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
                var path = $"JsonData/New_3D_Player";
                JArray jArray = await LoadJsonArrayAsync(path);
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
        var path = $"JsonData/New_3D_PlayerSkill";
        JArray jArray = await LoadJsonArrayAsync(path);
        
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
        var path = "JsonData/New_3D_PlayerWeapon";
        JArray jArray = await LoadJsonArrayAsync(path);

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

    public void ParsePlayerWeaponData()
    {
        var path = "JsonData/New_3D_PlayerWeapon";
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



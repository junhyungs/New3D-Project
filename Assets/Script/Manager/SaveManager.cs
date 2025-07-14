using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using EnumCollection;

public class SaveManager : Singleton<SaveManager>
{
    public static int SaveIndex;
    public static string DirectoryPath
    {
        get
        {
            var path = Path.Combine(Application.persistentDataPath, $"SavePlayerData{SaveIndex}");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }

    public class KnownTypesBinder : ISerializationBinder
    {
        public List<Type> KnownTypes { get; set; }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            foreach(var type in KnownTypes)
            {
                if(type.Name == typeName)
                    return type;
            }

            throw new JsonSerializationException($"{typeName} is not a known Type.");
        }
    }

    public async UniTask SaveAllData()
    {
        string directoryPath = DirectoryPath;
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var task_Group1 = UniTask.WhenAll(
            SavePlayer(directoryPath),
            SaveSkillUpgrade(directoryPath),
            SaveAbilityUpgrade(directoryPath)
            );

        await task_Group1;

        var task_Group2 = UniTask.WhenAll(
            SaveMap(directoryPath),
            SaveInventory(directoryPath)
            );

        await task_Group2;
    }

    private async UniTask WriteAllText(string path, Data data, JsonSerializerSettings setting = null)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented, setting);
        await File.WriteAllTextAsync(path, json);
    }

    public async UniTask SavePlayer(string directoryPath)
    {
        var key = DataKey.Player.ToString();
        var playerSaveData = DataManager.Instance.GetData(key) as PlayerSaveData;
        var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        playerSaveData.Date = date;
        var path = Path.Combine(directoryPath, "PlayerData.json");
        await WriteAllText(path, playerSaveData);
    }

    public async UniTask SaveMap(string directoryPath)
    {
        var playerObject = PlayerManager.Instance.PlayerObject;
        var playerTransform = playerObject.transform;

        var key = DataKey.Map_Data.ToString();
        var mapData = DataManager.Instance.GetData(key) as MapData;
        mapData.PlayerPosition = playerTransform.position;
        mapData.PlayerRotation = playerTransform.rotation;

        var setting = GetMapSetting();
        var path = Path.Combine(directoryPath, "MapData.json");
        await WriteAllText(path, mapData, setting);
    }

    public async UniTask SaveSkillUpgrade(string directoryPath)
    {
        var key = DataKey.SkillUpgrade.ToString();
        var data = DataManager.Instance.GetData(key) as PlayerUpgradeData_Skill;

        var path = Path.Combine(directoryPath, "UpgrageSkill.json");
        await WriteAllText(path, data);
    }

    public async UniTask SaveAbilityUpgrade(string directoryPath)
    {
        var key = DataKey.AbilityUpgrade.ToString();
        var data = DataManager.Instance.GetData(key) as PlayerUpgradeData_Ability;

        var path = Path.Combine(directoryPath, "UpgradeAbility.json");
        await WriteAllText(path, data);
    }

    public async UniTask SaveInventory(string directoryPath)
    {
        var key = DataKey.Inventory_Data.ToString();
        var data = DataManager.Instance.GetData(key) as PlayerInventoryData;

        var player = PlayerManager.Instance.PlayerComponent;
        var playerWeapon = player.GetComponent<PlayerWeapon>();
        if(playerWeapon != null)
        {
            var currentWeaponData = playerWeapon.GetWeaponData();
            data.SaveWeapon = currentWeaponData;
        }

        var path = Path.Combine(directoryPath, "Inventory.json");
        await WriteAllText(path, data);
    }

    private string GetPath(string fileName, int index)
    {
        return Path.Combine(Application.persistentDataPath, $"SavePlayerData{index}", fileName);
    }

    private async UniTask<T> DeserializeObjectAsync<T>(string path, JsonSerializerSettings setting = null)
        where T : class
    {
        try
        {
            string json = await File.ReadAllTextAsync(path);
            T tData = JsonConvert.DeserializeObject<T>(json, setting);
            return tData;
        }
        catch
        {
            return null;
        }
    }

    public async UniTask<PlayerSaveData> LoadPlayerSaveDataAsync(int index)
    {
        string fileName = $"PlayerData.json";
        string filePath = GetPath(fileName, index);

        try
        {
            PlayerSaveData saveData = await DeserializeObjectAsync<PlayerSaveData>(filePath);   
            return saveData;
        }
        catch
        {
            return null; 
        }
    }

    public async UniTask<MapData> LoadMapSaveDataAsync()
    {
        string fileName = "MapData.json";
        string filePath = GetPath(fileName, SaveIndex);

        try
        {
            var setting = GetMapSetting();
            MapData mapData = await DeserializeObjectAsync<MapData>(filePath, setting);
            return mapData;
        }
        catch
        {
            return null;
        }
    }

    public async UniTask<PlayerUpgradeData_Skill> LoadPlayerUpgradeData_SkillAsync()
    {
        var fileName = "UpgrageSkill.json";
        var filePath = GetPath(fileName, SaveIndex);

        try
        {
            PlayerUpgradeData_Skill data = await DeserializeObjectAsync<PlayerUpgradeData_Skill>(filePath);
            return data;
        }
        catch
        {
            return null;
        }
    }

    public async UniTask<PlayerUpgradeData_Ability> LoadPlayerUpgradeData_AbilityAsync()
    {
        var fileName = "UpgradeAbility.json";
        var filePath = GetPath(fileName, SaveIndex);

        try
        {
            PlayerUpgradeData_Ability data = await DeserializeObjectAsync<PlayerUpgradeData_Ability>(filePath);
            return data;
        }
        catch
        {
            return null;
        }
    }

    public async UniTask<PlayerInventoryData> LoadPlayerInventoryDataAsync()
    {
        var fileName = "Inventory.json";
        var filePath = GetPath(fileName, SaveIndex);

        try
        {
            PlayerInventoryData data = await DeserializeObjectAsync<PlayerInventoryData>(filePath);
            return data;
        }
        catch
        {
            return null;
        }
    }

    private JsonSerializerSettings GetMapSetting()
    {
        var setting = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            SerializationBinder = new KnownTypesBinder
            {
                KnownTypes = new List<Type>
                {
                    typeof(Level_0_progress),
                    typeof(Level_1_progress),
                    typeof(Level_2_progress)
                }
            }
        };

        return setting;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System;
using Cysharp.Threading.Tasks;

public class SaveManager : Singleton<SaveManager>
{
    public static int SaveIndex;

    public void SavePlayerData(PlayerSaveData playerSaveData)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        playerSaveData.Date = date;

        string json = JsonConvert.SerializeObject(playerSaveData, Formatting.Indented);

        string directoryPath = Path.Combine(Application.persistentDataPath, "SavePlayerData");
        if(!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string path = Path.Combine(directoryPath, $"PlayerData{SaveIndex}.json");
        File.WriteAllText(path, json);

        Debug.Log("SavePlayerData");
    }

    public async UniTask<PlayerSaveData> LoadPlayerSaveDataAsync(int index)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "SavePlayerData", $"PlayerData{index}.json");

        try
        {
            string json = await File.ReadAllTextAsync(filePath);
            PlayerSaveData saveData = JsonConvert.DeserializeObject<PlayerSaveData>(json);

            return saveData;
        }
        catch
        {
            return null; 
        }
    }
}

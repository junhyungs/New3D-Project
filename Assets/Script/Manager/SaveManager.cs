using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System;

public class SaveManager : Singleton<SaveManager>
{
    //public async Task<bool> SaveData(Data data)
    //{
    //    return false;
    //}

    public async Task<PlayerSaveData> LoadPlayerSaveDataAsync(int index)
    {
        string filePath = Application.persistentDataPath + "PlayerData" + index + ".json";

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

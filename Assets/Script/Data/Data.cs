using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GameData
{
    [System.Serializable]
    public class Data { }

    public class MapData : Data
    {
        public Dictionary<string, MapProgress> ProgressDictionary { get; set; }
        public string MapAddressablesKey { get; set; }
        //Newtonsoft.Json에서 Vector3, Quaternion직렬화 지원. 이외에는 x,y,z,w 등으로 직접 기록해야함.
        public Vector3 PlayerPosition { get; set; }
        public Quaternion PlayerRotation { get; set; }  
        public MapData(Dictionary<string, MapProgress> progressDictionary)
        {
            ProgressDictionary = progressDictionary;
        }
    }

    public class MapProgress
    {
        public bool Initialize { get; set; }
        public Dictionary<ItemType, bool> CollectedItemsDictionary{ get; set; }
    }

    public class Level_0_progress : MapProgress
    {
        public bool HallCrowScene { get; set; }
    }

    public class Level_1_progress : MapProgress
    {
        public bool ClearBoss { get; set; }
    }

    public class Level_2_progress : MapProgress
    {

    }

    public class DialogData : Data
    {
        private Dictionary<string, Dialog> _dialogDictionary;

        public Dialog GetMyDialog(string npcName)
        {
            if(_dialogDictionary.TryGetValue(npcName, out var dialog))
            {
                return dialog;
            }

            return null;
        }

        public DialogData(Dictionary<string, Dialog> dialogDictionary)
        {
            _dialogDictionary = dialogDictionary;
        }
    }

    public class Dialog
    {
        public string Name { get; }
        public List<string> StoryList { get; }
        public List<string> LoopList { get; }
        public List<string> EndList { get; }
        public Dialog(string name, List<string> storyList,
            List<string> loopList, List<string> endList)
        {
            Name = name;
            StoryList = storyList;
            LoopList = loopList;
            EndList = endList;
        }
    }

    public class ItemData : Data { }

    public class SkillData : ItemData
    {
        public float Speed { get; set; }
        public int Damage { get; set; }
        public int Cost { get; set; }
        public float FlightTime { get; set; }
        public SkillData(float speed, int damage, int cost, float flightTime)
        {
            Speed=speed;
            Damage=damage;
            Cost=cost;
            FlightTime=flightTime;
        }
    }

    public class WeaponData : ItemData
    {
        public ItemType WeaponType { get; set; }
        public string WeaponName { get; set; }
        public string ItemDescription { get; set; }
        public int Damage { get; set; }
        public Vector3 Range { get; set; }
        public WeaponData(int damage, Vector3 range, string weaponName, string itemDescription, ItemType itemType)
        {
            Damage=damage;
            Range=range;
            WeaponName=weaponName;
            ItemDescription=itemDescription;
            WeaponType = itemType;
        }
    }

    public class InventoryItemData : ItemData
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public InventoryItemData(string itemName, string itemDescription)
        {
            ItemName = itemName;
            ItemDescription = itemDescription;
        }
    }

    public class TrinketItemData : ItemData
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public TrinketItemData(string itemName, string itemDescription)
        {
            ItemName = itemName;
            ItemDescription = itemDescription;
        }
    }

    public class ScreenResolutionData : Data
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public ScreenResolutionData(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class PlayerSaveData : Data
    {
        public string ID { get; set; }
        public string Date { get; set; }
        public int Power {  get; set; }
        public float Speed { get; set; }
        public int Health { get; set; }
        public PlayerConstantData ConstantData { get; set; }
    }

    public class PlayerConstantData : Data
    {
        public float RollSpeed { get; set; }
        public float LadderSpeed { get; set; }
        public float SpeedChangeValue { get; set; }
        public float SpeedOffSet { get; set; }
        public float DashSpeed { get; set; }

        public PlayerConstantData(float rollSpeed, float ladderSpeed,
            float speedChangeValue, float speedOffSet, float dashSpeed)
        {
            RollSpeed = rollSpeed;
            LadderSpeed = ladderSpeed;
            SpeedChangeValue = speedChangeValue;
            SpeedOffSet = speedOffSet;
            DashSpeed = dashSpeed;
        }
    }

    public class PlayerUpgradeData_Skill : Data
    {
        public int DamageUpgrade { get; set; }
        public PlayerUpgradeData_Skill(int damageUpgrade)
        {
            DamageUpgrade = damageUpgrade;
        }
    }

    public class PlayerUpgradeData_Ability : Data
    {
        public int PowerUpgrade { get; set; }
        public float SpeedUpgrade { get; set; }

        public PlayerUpgradeData_Ability(int powerUpgrade, float speedUpgrade)
        {
            PowerUpgrade = powerUpgrade;
            SpeedUpgrade = speedUpgrade;
        }
    }

    public class PlayerInventoryData : Data
    {
        public bool InitInventory { get; set; }
        public WeaponData SaveWeapon { get; set; }
        public Dictionary<ItemType, WeaponData> WeaponDataDictionary { get; private set; }
        public Dictionary<ItemType, InventoryItemData> InventoryDataDictionary { get; private set; }
        public Dictionary<ItemType, TrinketItemData> TrinketDataDictionary { get; private set; }
        public int SeedCount { get; set; }
        public int SoulCount { get; set; }

        public WeaponData GetWeaponData(ItemType itemType)
        {
            if(WeaponDataDictionary.TryGetValue(itemType, out var weaponData))
                return weaponData;
            return null;
        }

        public InventoryItemData GetInventoryItemData(ItemType itemType)
        {
            if(InventoryDataDictionary.TryGetValue(itemType, out var inventoryItemData))
                return inventoryItemData;
            return null;
        }

        public TrinketItemData GetTrinketItemData(ItemType itemType)
        {
            if(TrinketDataDictionary.TryGetValue(itemType, out var trinketItemData))
                return trinketItemData;
            return null;
        }
        
        public PlayerInventoryData()
        {
            WeaponDataDictionary = new Dictionary<ItemType, WeaponData>();
            InventoryDataDictionary = new Dictionary<ItemType, InventoryItemData>();
            TrinketDataDictionary = new Dictionary<ItemType, TrinketItemData>();
        }
    }
}

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
        public string CurrentMapObjectName { get; set; }
        public MapData(Dictionary<string, MapProgress> progressDictionary)
        {
            ProgressDictionary = progressDictionary;
        }
    }

    public class MapProgress
    {
        public bool Initialize { get; set; }
        public Vector3 PlayerPosition { get; set; }
        public Dictionary<ItemType, bool> CollectedItemsDictionary{ get; set; }
    }

    public class Level_0 : MapProgress
    {
        public bool HallCrowScene { get; set; }
    }

    public class Level_1 : MapProgress
    {
        public bool ClearBoss { get; set; }
    }

    public class Level_2 : MapProgress
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

    public class PlayerWeaponData : Data
    {
        public string Id { get; set; }
        public int Damage { get; set; }
        public Vector3 Range { get; set; }

        public PlayerWeaponData(string id, int damage, Vector3 range)
        {
            Id = id;
            Damage = damage;
            Range = range;
        }
    }

    public class ItemDescriptionData : Data
    {
        public string Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }

        public ItemDescriptionData(string id, string itemName, string description)
        {
            Id = id;
            ItemName = itemName;
            Description = description;
        }
    }

    public class PathData : Data
    {
        public string ID { get; set; }
        public string Path { get; set; }

        public PathData(string id, string path)
        {
            ID = id;
            Path = path;
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

    public class PlayerSkillData : Data
    {
        public string ID { get; set; }
        public float ProjectileSpeed { get; set; }
        public int ProjectileDamage { get; set; }
        public int ProjectileCost { get; set; }
        public float FlightTime { get; set; }
        public PlayerSkillData(string id, float projectileSpeed, int projectileDamage,
            int projectileCost, float flightTime)
        {
            ID = id;
            ProjectileSpeed = projectileSpeed;
            ProjectileDamage = projectileDamage;
            ProjectileCost = projectileCost;
            FlightTime = flightTime;
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
        public Dictionary<ItemType, ItemDescriptionData> DescriptionDataDictionary { get; private set; }
        public Dictionary<ItemType, PlayerWeaponData> WeaponDataDictionary { get; private set; }
        public HashSet<ItemType> EquipItemSet { get; private set; }
        public int SeedCount { get; set; }
        public int SoulCount { get; set; }
        
        public PlayerInventoryData()
        {
            DescriptionDataDictionary = new Dictionary<ItemType, ItemDescriptionData>();
            WeaponDataDictionary = new Dictionary<ItemType, PlayerWeaponData>();
            EquipItemSet = new HashSet<ItemType>();
        }
    }
}

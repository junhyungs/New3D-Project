using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace GameData
{
    [System.Serializable]
    public class Data { }

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
}

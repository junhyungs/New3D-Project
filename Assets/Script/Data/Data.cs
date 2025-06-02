using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class Data { }

    public class PlayerSkillData : Data
    {
        public string ID { get; set; }
        public float ProjectileSpeed { get; set; }
        public int ProjectileDamage { get; set; }
        public int ProjectileCost { get; set; }
        public float MaxDistance { get; set; }
        public PlayerSkillData(string id, float projectileSpeed, int projectileDamage,
            int projectileCost, float maxDistance)
        {
            ID = id;
            ProjectileSpeed = projectileSpeed;
            ProjectileDamage = projectileDamage;
            ProjectileCost = projectileCost;
            MaxDistance = maxDistance;
        }
    }

    public class  PathData : Data
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
}

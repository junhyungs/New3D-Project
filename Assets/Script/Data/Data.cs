using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class Data { }

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
            float speedChangeValue, float speedOffSet, float dashDistance)
        {
            RollSpeed = rollSpeed;
            LadderSpeed = ladderSpeed;
            SpeedChangeValue = speedChangeValue;
            SpeedOffSet = speedOffSet;
            DashSpeed = dashDistance;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class Data { }

    public class PlayerBaseData : Data
    {
        public string Id {  get; set; }

    }

    public class PlayerSaveData : Data
    {
        public string Id { get; set; }
        public string Date { get; set; }
    }
}

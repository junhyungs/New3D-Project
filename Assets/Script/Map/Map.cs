using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapComponent
{
    public abstract class Map : MonoBehaviour
    {
        public Dictionary<string, MapProgress> ProgressDictionary { get; set; }
        public virtual void Initialize(Dictionary<string , MapProgress> progressDictionary) { }
    }
}

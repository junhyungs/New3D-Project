using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace GameData
{
    public abstract class ScriptableData : ScriptableObject
    {
        [Header("Key"), SerializeField]
        protected ScriptableDataKey _key;
        public abstract ScriptableDataKey Key { get; }
    }
}


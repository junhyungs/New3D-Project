using GameData;
using MapComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EventClass
{
    [Serializable]
    public class InteractObject
    {
        [Header("InteractObjects")]
        public GameObject[] InteractObjects;
        [Header("LastTargetObject")]
        public GameObject LastTargetObject;

        public HashSet<string> ClearedObjects;
    }

    public class DoorEvent<TMapType>
        where TMapType : MapProgress
    {
        public DoorEvent()
        {
            
        }

        public void Complete()
        {
            
        }

        private void RemoveHitSet(HitTrigger hitTrigger)
        {
           
        }
    }
}


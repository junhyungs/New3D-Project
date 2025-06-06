using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventroyUI
{
    [System.Serializable]
    public class Info<T> where T : Enum
    {
        [Header("Type")]
        public T Type;
        [Header("SlotObject")]
        public GameObject SlotObject;
    }
}


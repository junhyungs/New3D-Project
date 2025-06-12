using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI
{
    [System.Serializable]
    public class Info<T> where T : Enum
    {
        [Header("Type")]
        public T Type;
        [Header("InfoObject")]
        public GameObject InfoObject;
    }
}


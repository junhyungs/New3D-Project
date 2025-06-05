using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventroyUI
{
    [System.Serializable]
    public class IconInfo
    {
        [Header("IconImage")]
        public Image Icon;

        [Header("Panel")]
        public GameObject Panel;
    }
}

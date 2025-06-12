using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using SO;
using EnumCollection;

namespace PlayerComponent
{
    [System.Serializable]
    public class EquipTransformInfo
    {
        [Header("EquipTransform")]
        public Transform Holster;
        public Transform WeaponL;
        public Transform WeaponR;
    }

    public class PlayerWeaponSystem : MonoBehaviour
    {
        [Header("EquipTransform"), SerializeField]
        private EquipTransformInfo _equipTransformInfo;

        private void Awake()
        {
            WeaponManager.Instance.EquipTransformInfo = _equipTransformInfo;
        }
    }
}


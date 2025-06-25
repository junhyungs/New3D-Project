using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using SO;
using EnumCollection;

namespace PlayerComponent
{
    public struct WeaponInfo
    {
        public Transform Parent;
        public PlayerHand Hand;

        public WeaponInfo(Transform parent, PlayerHand hand)
        {
            this.Parent = parent;
            this.Hand = hand;
        }
    }

    public class PlayerWeaponSystem : MonoBehaviour
    {
        [Header("EquipTransform"), SerializeField]
        private EquipTransformInfo _equipTransformInfo;

        private void Awake()
        {
            var weaponInfos = new WeaponInfo[]
            {
                new(_equipTransformInfo.Holster, PlayerHand.Idle),
                new(_equipTransformInfo.WeaponR, PlayerHand.Right),
                new(_equipTransformInfo.WeaponL, PlayerHand.Left),
                new(_equipTransformInfo.WeaponL, PlayerHand.Charge_L),
                new(_equipTransformInfo.WeaponR, PlayerHand.Charge_R),
            };

            WeaponManager.Instance.WeaponInfos = weaponInfos;
        }
    }
}


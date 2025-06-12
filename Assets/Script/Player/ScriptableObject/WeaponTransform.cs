using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace SO
{
    [CreateAssetMenu(fileName = "WeaponTransformSO", menuName = "ScriptableObject/Player/WeaponTransform")]
    public class WeaponTransform : ScriptableObject
    {
        [Header("WeaponType"), SerializeField]
        private ItemType _weaponType;
        [Header("HandType"), SerializeField]
        private PlayerHand _handType;
        [Header("Rotation"), SerializeField]
        private Vector3 _localEulerAngles;
        [Header("Position"), SerializeField]
        private Vector3 _localPosition;

        public ItemType WeaponType
        {
            get => _weaponType;
            set => _weaponType = value;
        }
        public PlayerHand HandType
        {
            get => _handType;
            set => _handType = value;
        }
        public Vector3 LocalEulerAngles
        {
            get => _localEulerAngles;
            set => _localEulerAngles = value;
        }
        public Vector3 LocalPosition
        {
            get => _localPosition;
            set => _localPosition = value;
        }
    }
}


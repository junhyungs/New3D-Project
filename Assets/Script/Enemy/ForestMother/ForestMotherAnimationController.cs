using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class ForestMotherAnimationController : MonoBehaviour
    {
        //parameter 명명 규칙 Trigger = "parameterName", Boolean = "parameterName_true OR false"
        private enum MotherParameter
        {
            Slam_Trigger,
            Hyper_Trigger,
            Lift_Trigger,
            LiftFall_Trigger,
            Shoot_Trigger,
            IdleSpin_Bool,
            StartSlam_Trigger,
            StartHyper_Bool,
            StartSlamSlow_Bool,
            Lift_DamageRight_Bool,
            Lift_DamageLeft_Bool,
            Death_Trigger
        }

        [Header("LowerBodyController")]
        [SerializeField] private LowerBodyController _lowerBodyController;
        private Dictionary<string, Delegate> _animationDictionary;
        private ForestMotherVineController _vineController;
        private Animator _animator;

        private void Awake()
        {
            GetComponent();
            BindAnimation();
        }

        private void GetComponent()
        {
            _animator = GetComponent<Animator>();
            _vineController = GetComponent<ForestMotherVineController>();
            _lowerBodyController.Owner = GetComponent<ForestMother>();
        }

        private void BindAnimation()
        {
            _animationDictionary = new Dictionary<string, Delegate>();

            var enumValues = Enum.GetValues(typeof(MotherParameter));
            foreach(MotherParameter enumValue in enumValues)
            {
                var parameter = enumValue.ToString();
                var animatorStringToHash = Animator.StringToHash(parameter);

                if (!_animationDictionary.ContainsKey(parameter))
                {
                    var index = parameter.LastIndexOf('_') + 1;
                    var parameterType = parameter.Substring(index);
                    switch (parameterType)
                    {
                        case "Trigger":
                            _animationDictionary[parameter] = (Action)(() => _animator.SetTrigger(animatorStringToHash));
                            break;
                        case "Bool":
                            _animationDictionary[parameter] = (Action<bool>)((value) => _animator.SetBool(animatorStringToHash, value));
                            break;
                    }
                }
            }
        }

        public void PlayAnimation(string value)
        {
            var index = value.Length;
            if (!value.Contains("Trigger"))
                index = value.LastIndexOf("_");

            var parameter = value.Substring(0, index);
            if(_animationDictionary.TryGetValue(parameter, out var del))
            {
                switch (del)
                {
                    case Action action:
                        action?.Invoke();
                        break;
                    case Action<bool> action:
                        var boolStr = value.Substring(index + 1);
                        if (!string.IsNullOrEmpty(boolStr) && bool.TryParse(boolStr, out bool result))
                            action?.Invoke(result);
                        break;
                }
            }
        }

        public void OnRightCollider(string enabled)
        {
            EnabledCollider(enabled, VineType.Right);
        }

        public void OnLeftCollider(string enabled)
        {
            EnabledCollider(enabled, VineType.Left);
        }

        public void OnAllCollider(string enabled)
        {
            EnabledCollider(enabled);
        }

        private void EnabledCollider(string enabled, VineType vineType = VineType.Default)
        {
            if (!string.IsNullOrEmpty(enabled) && bool.TryParse(enabled, out bool result))
            {
                if(vineType == VineType.Default)
                    _vineController.UseAllCollider(result);
                else
                    _vineController.UseCollider(vineType, result);
            }
        }

        public void RotateByAngle()
        {
            RotateByAngle(75f);
        }

        public void RotateByAngle(float rotSpeed, Action callBack = null)
        {
            _lowerBodyController.RotateByAngle(rotSpeed, callBack);
        }

        public void RotateForDuration(float rotSpeed, float seconds,  Action callBack = null)
        {
            _lowerBodyController.RotateForDuration(rotSpeed, seconds, callBack);
        }

        public void InfinitiRotation(Vector3 rotDir, bool value, float rotSpeed = 75f)
        {
            _lowerBodyController.InfinitiRotation(rotDir, value, rotSpeed);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class Lift : ForestMother_Pattern
    {
        private VineType _currentVine = VineType.Default;

        private float _currentTime;
        private float _lastShootTime;
        private const float MaxTime = 10f;

        private const string IgnoreHit = "IgnoreHit";
        private const string Enemy = "Enemy";

        public override void Start()
        {
            IsRunning = true;
            ChangeVineMaterial(VineMaterial.Vine, true);
            SetLayer(IgnoreHit);

            SetIsTrigger(true);
            _owner.StartCoroutine(WaitForAnimation());
        }

        public override void Exit()
        {
            _currentVine = VineType.Default;
            _currentTime = 0f;

            SetLayer(Enemy);
            ChangeVineMaterial(VineMaterial.Body, false);
            SetIsTrigger(false);
        }

        public override IEnumerator WaitForAnimation()
        {
            PlayAnimation(MotherParameterKey.Lift_Trigger);
            yield return new WaitUntil(() =>
            {
                var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(2);
                return stateInfo.IsName("LiftIdle");
            });

            while(true)
            {
                _currentTime += Time.deltaTime;
                if (_currentVine != VineType.Default ||
                    _currentTime >= MaxTime)
                {
                    _lastShootTime = 0f;
                    break;
                }
                
                if (_currentTime - _lastShootTime >= 4f)
                {
                    PlayAnimation(MotherParameterKey.Shoot_Trigger);
                    _lastShootTime = Time.time;
                }

                yield return null;
            }

            if(_currentVine == VineType.Default)
                PlayAnimation(MotherParameterKey.LiftFall_Trigger);

            yield return new WaitUntil(() =>
            {
                var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(2);
                return stateInfo.IsName("Fall") && stateInfo.normalizedTime >= 0.8f;
            });

            yield return _delay;
            IsRunning = false;
        }

        private void VineDamage(VineType type, Material material, int health)
        {
            if (health >= 0)
                _owner.StartCoroutine(_owner.IntensityChange(material));

            if(_currentVine != VineType.Default &&
                health <= 0)
            {
                VineAnimation(MotherParameterKey.Lift_DamageLeft_Bool_false,
                    MotherParameterKey.Lift_DamageRight_Bool_false);
            }
            else if (_currentVine == VineType.Default &&
                health < _property.Data.VineHealth / 2)
            {
                _currentVine = type;
                VineAnimation(MotherParameterKey.Lift_DamageLeft_Bool_true,
                    MotherParameterKey.Lift_DamageRight_Bool_true);
            }    
        }

        private void VineAnimation(MotherParameterKey left, MotherParameterKey right)
        {
            switch (_currentVine)
            {
                case VineType.Left:
                    PlayAnimation(left);
                    break;
                case VineType.Right:
                    PlayAnimation(right);
                    break;
            }
        }

        private void ChangeVineMaterial(VineMaterial vineMaterial, bool value)
        {
            foreach (var vine in _owner.Vines)
            {
                vine.ChangeMaterial(vineMaterial, _property.Data);
                if (vine is IVineEvent vineEvent)
                    vine.Register(VineDamage, value);
            }
        }

        private void SetLayer(string LayerName) =>
            _owner.gameObject.layer = LayerMask.NameToLayer(LayerName);
    }
}


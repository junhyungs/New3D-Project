using EnumCollection;
using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace EnemyComponent
{
    public class EnemyState<TProperty, TOwner, TStateMachine, TEnum> 
        where TProperty : IPropertyBase, IStateMachine<TStateMachine, TEnum>
        where TOwner : Enemy<TProperty>
        where TStateMachine : IStateController<TEnum>
        where TEnum : Enum
    {
        public EnemyState(TOwner owner)
        {
            _owner = owner;
            _property = owner.Property;
        }

        protected TOwner _owner;
        protected TProperty _property;
        protected const string MATERIAL_PROPERTY = "_NoiseValue";

        protected float GetRange(MonsterDataSO data)
        {
            return _property.IsSpawn ? 
                data.Spawn_DetectionRange : data.DetectionRange;
        }

        protected void AgentSetting(float stoppingDistance, float speed, float acceleration = 8f)
        {
            _property.NavMeshAgent.stoppingDistance = stoppingDistance;
            _property.NavMeshAgent.speed = speed;
            _property.NavMeshAgent.acceleration = acceleration;
        }

        protected Transform FindPlayer(MonsterDataSO data)
        {
            var playerLayer = LayerMask.GetMask("Player");
            var range = GetRange(data);
            var results = new Collider[1];
            
            var count = Physics.OverlapSphereNonAlloc(_owner.transform.position,
                range, results, playerLayer);
            if (count > 0)
            {
                var targetTransform = results[0].transform;
                return targetTransform;
            }
            else
                return null;
        }

        protected void Death(int stringToHash)
        {
            _owner.StopAllCoroutines();
            _property.NavMeshAgent.isStopped = true;

            if (IsMaterialValueBelow(5))
                _property.CopyMaterial.SetFloat(MATERIAL_PROPERTY, 0.5f);

            _property.Animator.SetTrigger(stringToHash);
            _owner.StartCoroutine(DissolveEffect(3f, -0.5f));
        }

        private IEnumerator DissolveEffect(float duration, float targetValue)
        {
            _owner.StartCoroutine(
                _owner.DissolveEffect(
                    _property.CopyMaterial,
                    duration,
                    targetValue,
                    MATERIAL_PROPERTY)
                );
            yield return new WaitUntil(() => IsMaterialValueBelow(-5));
            //TODO EnemyPool·Î ¹ÝÈ¯.
        }

        private bool IsMaterialValueBelow(int compareValue)
        {
            var getFloat = _property.CopyMaterial.GetFloat(MATERIAL_PROPERTY) * 10;
            return getFloat <= compareValue;
        }
    }

    public struct PlayerScan
    {
        public PlayerScan(float intervalTime)
        {
            IntervalTime = intervalTime;
            NextScanTime = 0f;
        }
            
        private float NextScanTime;
        private float IntervalTime;

        public void InitPlayerScan(float currentTime) =>
            NextScanTime = currentTime + IntervalTime;

        public bool IsReady(float currentTime)
        {
            if (currentTime >= NextScanTime)
            {
                NextScanTime = Time.time + IntervalTime;
                return true;
            }
                
            return false;
        }
    }
}


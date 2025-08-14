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

        protected float GetRange(EnemyDataSO data)
        {
            return _property.IsSpawn ? 
                data.Spawn_DetectionRange : data.DetectionRange;
        }

        protected void AgentSetting(float stoppingDistance, float speed)
        {
            _property.NavMeshAgent.stoppingDistance = stoppingDistance;
            _property.NavMeshAgent.speed = speed;
        }

        protected Transform FindPlayer(EnemyDataSO data)
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


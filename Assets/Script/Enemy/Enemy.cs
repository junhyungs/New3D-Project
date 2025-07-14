using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace EnemyComponent
{
    public class Enemy<TStateMachine, TEnum> : MonoBehaviour 
        where TStateMachine : IEnemyStateController<TEnum>
        where TEnum : Enum
    {
        public TStateMachine StateMachine { get; private set; }
        private WaitForSeconds _waitForIntensity = new WaitForSeconds(0.1f);
        
        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnAwake()
        {
            StateMachine = GetComponent<TStateMachine>();
        }

        protected virtual void OnStart()
        {
            StateMachine.InitializeOnStart();
        }

        public Material Copy(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            var sharedMaterial = skinnedMeshRenderer.sharedMaterial;
            var copyMaterial = Instantiate(sharedMaterial);
            return copyMaterial;
        }

        public Material Copy(MeshRenderer meshRenderer)
        {
            var sharedMaterial = meshRenderer.sharedMaterial;
            var copyMaterial = Instantiate(sharedMaterial);
            return copyMaterial;
        }

        public IEnumerator DissolveEffect(Material targetMaterial, float maxTime, 
            float targetValue, string propertyName)
        {
            var elapsedTime = 0f;
            var startValue = targetMaterial.GetFloat(propertyName);
            while(elapsedTime < maxTime)
            {
                elapsedTime += Time.deltaTime;
                var colorValue = Mathf.Lerp(startValue, targetValue, elapsedTime / maxTime);
                targetMaterial.SetFloat(propertyName, colorValue);
                yield return null;
            }

            targetMaterial.SetFloat(propertyName, targetValue);
        }

        public IEnumerator IntensityChange(Material targetMaterial, float baseValue = 2f,
            float power = 3f)
        {
            var color = targetMaterial.GetColor("_Color");
            var upColor = color * Mathf.Pow(baseValue, power);

            targetMaterial.SetColor("_Color", upColor);
            yield return _waitForIntensity;
            targetMaterial.SetColor("_Color", color);
        }
    }
}


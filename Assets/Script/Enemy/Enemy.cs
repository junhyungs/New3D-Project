using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace EnemyComponent
{
  

    public abstract class Enemy<TProperty> : MonoBehaviour, ITakeDamage
        where TProperty : IPropertyBase
    {
        [Header("Material")]
        [SerializeField] private Material _originalMaterial;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;

        private WaitForSeconds _waitForIntensity = new WaitForSeconds(0.1f);
        public TProperty Property { get; private set; } 

        private void Awake()
        {
            OnAwakeEnemy();
        }

        protected virtual void OnAwakeEnemy()
        {
            Property = CreateProperty();
            MaterialSetting();
        }

        private void OnEnable()
        {
            OnEnableEnemy();
        }

        protected virtual void OnEnableEnemy()
        {
            Property.NavMeshAgent.isStopped = false;
        }

        protected abstract TProperty CreateProperty();
        protected abstract void Death();

        private void MaterialSetting()
        {
            if (_skinnedMeshRenderers == null)
                return;

            var copyMaterial = Instantiate(_originalMaterial);
            for(int i = 0; i < _skinnedMeshRenderers.Length; i++)
            {
                var renderer = _skinnedMeshRenderers[i];
                var sharedMaterials = renderer.sharedMaterials;
                var array = new Material[sharedMaterials.Length];

                for (int k = 0; k < sharedMaterials.Length; k++)
                    array[k] = copyMaterial;

                renderer.materials = array;
            }

            Property.CopyMaterial = copyMaterial;
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

        public virtual void TakeDamage(int damage)
        {
            Property.Health -= damage;
            if (Property.Health <= 0)
                Death();
            else
                StartCoroutine(IntensityChange(Property.CopyMaterial));
        }
    }
}


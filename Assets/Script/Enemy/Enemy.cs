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
        [SerializeField] protected Material _originalMaterial;
        [SerializeField] protected Renderer[] _renderers;

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
            Property.CopyMaterial.SetFloat("_NoiseValue", 0.5f);
        }

        private void Start()
        {
            OnStartEnemy();
        }

        protected virtual void OnStartEnemy() { }
        protected abstract TProperty CreateProperty();
        protected abstract void Death();

        protected virtual void MaterialSetting()
        {
            Property.CopyMaterial = InstantiateMaterial();
        }

        protected Material InstantiateMaterial()
        {
            var copyMaterial = Instantiate(_originalMaterial);
            for (int i = 0; i < _renderers.Length; i++)
            {
                var renderer = _renderers[i];
                var sharedMaterials = renderer.sharedMaterials;
                var array = new Material[sharedMaterials.Length];

                for (int k = 0; k < sharedMaterials.Length; k++)
                    array[k] = copyMaterial;

                renderer.materials = array;
            }

            return copyMaterial;
        }

        public IEnumerator DissolveEffect(Material targetMaterial, float maxTime,
            float targetValue, string propertyName)
        {
            var elapsedTime = 0f;
            var startValue = targetMaterial.GetFloat(propertyName);
            while (elapsedTime < maxTime)
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
            yield return new WaitForSeconds(0.1f);
            targetMaterial.SetColor("_Color", color);

            Debug.Log("IntensityExit");
        }

        public virtual void TakeDamage(int damage)
        {
            Debug.Log("TakeDamage");
            Property.Health -= damage;
            if (Property.Health <= 0)
                Death();
            else
                StartCoroutine(IntensityChange(Property.CopyMaterial));
        }

        public IEnumerator Test_WaitForSeconds(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action.Invoke();
        }
    }
}


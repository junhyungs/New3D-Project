using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameData;

namespace EnemyComponent
{
    public interface IVineEvent
    {
        void Register(Action<VineType, Material, int> action, bool register);
    }
    public enum VineType { Left, Right, Default };
    public enum VineMaterial { Body, Vine };
    public class Vine : MonoBehaviour, ITakeDamage, IVineEvent
    {
        [Header("VineType")]
        [SerializeField] private VineType _type;
        [Header("SkinnedMeshRenderer")]
        [SerializeField] private SkinnedMeshRenderer _vineRenderer;

        private CapsuleCollider _collider;
        private Dictionary<VineMaterial, Material> _materialDictionary;
        private Action<VineType, Material, int> _vineAction;

        private int _vineHealth;
        private VineMaterial _currentMaterial;

        public void TakeDamage(int damage)
        {
            _vineHealth -= damage;
            if(_vineHealth >= 0)
            {
                var material = _materialDictionary[_currentMaterial];
                _vineAction?.Invoke(_type, material, _vineHealth);
            }
        }

        public void InitVine(Material[] materials, ForestMotherSO data)
        {
            _materialDictionary = new Dictionary<VineMaterial, Material>()
            {
                {VineMaterial.Body, materials[0]},
                {VineMaterial.Vine, materials[1]}
            };

            ChangeMaterial(VineMaterial.Body, data);
            SetVineHealth(data);
        }

        public void ChangeMaterial(VineMaterial materialType, ForestMotherSO data)
        {
            bool isTrigger = materialType == VineMaterial.Body ?
                false : true;
            if (_collider == null)
                _collider = GetComponent<CapsuleCollider>();
            _collider.isTrigger = isTrigger;

            SetVineHealth(data);
            var material = _materialDictionary[materialType];
            if (material != null)
            {
                _currentMaterial = materialType;
                _vineRenderer.material = material;
            }
        }

        private void SetVineHealth(ForestMotherSO data)
        {
            _vineHealth = data.VineHealth;
        }

        public void Register(Action<VineType, Material, int> action, bool register)
        {
            if (register)
                _vineAction += action;
            else
                _vineAction -= action;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class ForestMotherVineController : MonoBehaviour
    {
        [Header("VineSetting")]
        [SerializeField] private MotherVineSetting[] _settings;
        private VineColliderController _colliderController;

        private void Awake()
        {
            var owner = GetComponent<ForestMother>();
            _colliderController = new VineColliderController(_settings, owner);
        }

        public void UseAllCollider(bool enabled) =>
            _colliderController.UseAllCollider(enabled);

        public void UseCollider(VineType type, bool enabled) =>
            _colliderController.UseCollider(type, enabled);
    }
}


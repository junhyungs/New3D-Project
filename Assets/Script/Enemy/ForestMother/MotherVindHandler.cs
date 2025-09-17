using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    [System.Serializable]
    public class MotherVineSetting
    {
        [Header("Bones")]
        public GameObject[] Bones;
        [Header("VineType")]
        public VineType VineType;

        [HideInInspector]
        public HashSet<int> OverlapSet;
        [HideInInspector]
        public SphereCollider[] SphereColliders;
    }

    public class VineColliderController
    {
        public VineColliderController(MotherVineSetting[] settings, ForestMother owner)
        {
            AddComponent(settings, owner);
        }

        private Dictionary<VineType, MotherVineSetting> _vineDictionary;

        public void UseAllCollider(bool enabled)
        {
            foreach(var setting in _vineDictionary.Values)
                EnabledCollider(setting, enabled);
        }

        public void UseCollider(VineType type, bool enabled)
        {
            var setting = GetSetting(type);
            EnabledCollider(setting, enabled);
        }

        private void EnabledCollider(MotherVineSetting setting, bool enabled)
        {
            if(setting != null)
            {
                foreach (var collider in setting.SphereColliders)
                    collider.enabled = enabled;

                if(!enabled)
                    setting.OverlapSet.Clear();
            }
        }

        private MotherVineSetting GetSetting(VineType type)
        {
            if(_vineDictionary.TryGetValue(type, out var setting))
                return setting;

            return null;
        }

        private void AddComponent(MotherVineSetting[] settings, ForestMother owner)
        {
            _vineDictionary = new Dictionary<VineType, MotherVineSetting>();

            var index = 0;
            foreach(var setting in settings)
            {
                setting.OverlapSet = new HashSet<int>();
                setting.SphereColliders = new SphereCollider[setting.Bones.Length];
                index++;

                for(int i = 0; i <  setting.Bones.Length; i++)
                {
                    var bone = setting.Bones[i];
                    if (bone == null)
                        continue;

                    setting.SphereColliders[i] = AddComponent_SphereCollier(bone);
                    AddComponent_Handler(bone, setting.OverlapSet, owner, index);
                }

                _vineDictionary.Add(setting.VineType, setting);
            }
        }

        private SphereCollider AddComponent_SphereCollier(GameObject bone)
        {
            var collider = bone.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 0.5f;
            collider.enabled = false;
            return collider;
        }

        private void AddComponent_Handler(GameObject bone, HashSet<int> set, ForestMother owner, int handlerIndex)
        {
            var handler = bone.AddComponent<MotherVindHandler>();
            handler.Init(set, owner, handlerIndex);
        }
    }

    public class MotherVindHandler : MonoBehaviour
    {
        private HashSet<int> _overlapSet;
        private ForestMother _owner;
        private int _index;
        
        public void Init(HashSet<int> set, ForestMother owner, int index)
        {
            _overlapSet = set;
            _owner = owner;
            _index = index;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_overlapSet.Contains(_index) ||
                !other.TryGetComponent(out ITakeDamage takeDamage))
                return;

            _overlapSet.Add(_index);

            var property = _owner.Property;
            takeDamage.TakeDamage(property.Data.Damage);
        }
    }
}


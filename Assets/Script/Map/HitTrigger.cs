using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapComponent
{
    public interface IHitTrigger
    {
        void HitTrigger();
    }


    public class HitTrigger : MapTrigger<SphereCollider>, IInteractionGameObject
    {
        [Header("GameObject")]
        [SerializeField] private GameObject[] _targetObjects;

        public event Action<HitTrigger> HitAction;
        public bool IsWeaponInteractable { get; set; } = true;

        public void Interact()
        {
            if (_targetObjects == null)
                return;

            foreach(var gameObject in _targetObjects)
            {
                if(gameObject != null &&
                    gameObject.TryGetComponent(out IHitTrigger hitTrigger))
                {
                    hitTrigger.HitTrigger();
                    HitAction?.Invoke(this);
                }
            }

            _collider.enabled = false;
        }
    }
}


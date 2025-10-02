using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapComponent
{
    public interface IHitInteraction
    {
        UniqueObjectID UniqueObjectID { get; }
        GameObject GameObject { get; }
        void OnHit();
        void ResetObject();
    }

    public interface IHitInteraction_Door : IHitInteraction
    {
        void CloseDoor();
    }


    public class HitTrigger : MapTrigger<SphereCollider>, IInteractionGameObject
    {
        [Header("GameObject")]
        [SerializeField] private GameObject[] _targetObjects;
        private List<IHitInteraction> _hitInteractions = new List<IHitInteraction>();

        public event Action<HitTrigger> HitAction;
        public bool IsWeaponInteractable { get; set; } = true;
        public List<IHitInteraction> HitInteractions => _hitInteractions;

        protected override void OnAwakeMapTrigger()
        {
            base.OnAwakeMapTrigger();
            if (_targetObjects == null)
                return;

            foreach (var gameObject in _targetObjects)
                if (gameObject != null &&
                    gameObject.TryGetComponent(out IHitInteraction hitInteraction))
                    _hitInteractions.Add(hitInteraction);
        }

        public void Interact()
        {
            if (_targetObjects == null)
                return;

            foreach(var hitInteraction in _hitInteractions)
                if (hitInteraction.GameObject.activeSelf)
                    hitInteraction.OnHit();

            HitAction?.Invoke(this);
            _collider.enabled = false;
        }
    }
}


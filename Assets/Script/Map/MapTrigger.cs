using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapComponent
{
    public class MapTrigger<TCollider> : MonoBehaviour
    {
        protected TCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<TCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            Trigger(other);
        }

        protected virtual void Trigger(Collider other) { }
    }
}


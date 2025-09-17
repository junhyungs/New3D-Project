using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapComponent
{
    public class MapTrigger : MonoBehaviour
    {
        protected SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
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


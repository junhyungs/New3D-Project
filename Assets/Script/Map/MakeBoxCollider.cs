using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapComponent
{
    public class MakeBoxCollider : MonoBehaviour
    {
        [Header("Height"), SerializeField] private float _height;
        private Transform[] _objectTransformArray;
        private Transform _colliderTransform;

        private void Awake()
        {
            var colliderLength = transform.childCount;
            _objectTransformArray = new Transform[colliderLength];

            var index = 0;
            foreach(Transform childTransform in transform)
                _objectTransformArray[index++] = childTransform;

            var parentObject = new GameObject("Collider");
            parentObject.transform.SetParent(transform);
            parentObject.transform.localPosition = Vector3.zero;
            _colliderTransform = parentObject.transform;
        }

        private void Start()
        {
            CreateBoxCollider();
        }

        private void CreateBoxCollider()
        {
            for(int i = 0; i < _objectTransformArray.Length - 1; i++)
            {
                var centerObject = new GameObject("Center");

                var center = (_objectTransformArray[i].position + _objectTransformArray[i + 1].position) / 2;
                centerObject.transform.position = center;

                var distance = Vector3.Distance(_objectTransformArray[i].position, _objectTransformArray[i + 1].position);

                var collider = centerObject.AddComponent<BoxCollider>();
                var size = collider.size;
                size.x = distance;
                size.y = _height != 0 ? _height : 1;
                size.z = 0.1f;
                collider.size = size;

                var lookPos = (_objectTransformArray[i].position - _objectTransformArray[i + 1].position).normalized;
                centerObject.transform.rotation = Quaternion.LookRotation(lookPos);
                centerObject.transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
                centerObject.transform.SetParent(_colliderTransform);
            }
        }
    }
}


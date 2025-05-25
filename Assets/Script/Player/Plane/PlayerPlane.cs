using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerComponent
{
    public class PlayerPlane : MonoBehaviour
    {
        private Plane _plane;
        public Vector3 Point { get; private set; }

        void Start()
        {
            _plane = new Plane(Vector3.up, gameObject.transform.position);
        }

        void Update()
        {
            SetMouseRayPoint();
        }

        private void SetMouseRayPoint()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (_plane.Raycast(ray, out float distance))
            {
                Point = ray.GetPoint(distance);
            }
        }
    }
}


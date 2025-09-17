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
            _plane = new Plane(Vector3.up, transform.position);
        }

        void Update()
        {
            _plane.SetNormalAndPosition(Vector3.up, transform.position);
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
    [Header("RotateSpeed"), SerializeField]
    private float _speed;

    private Vector3 _direction;
    private Quaternion _rotation;

    void Start()
    {
        _direction = transform.InverseTransformDirection(Vector3.up);
    }

    void Update()
    {
        _rotation = Quaternion.Euler(_direction * _speed * Time.unscaledDeltaTime);

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, transform.localRotation * _rotation,
            _speed * Time.unscaledDeltaTime);
    }
}

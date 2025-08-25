using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlimeWeapon : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxDistance;

    public int Damage { get; set; }
   
    private float _currentDistance;
    private Vector3 _direction;
    private Vector3 _startPos;

    public void StartMoveCoroutine()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        _direction = transform.right;
        _startPos = transform.localPosition;

        while (true)
        {
            var sign = _currentDistance > _maxDistance ?
                -1 : 1;
            Vector3 moveVector = _direction * (sign * _speed * Time.deltaTime);
            transform.Translate(moveVector);
            _currentDistance += _speed * Time.deltaTime;

            if(sign < 0 && Vector3.Distance(_startPos, transform.localPosition) <= 0.1f)
            {
                transform.localPosition = Vector3.zero;
                _currentDistance = 0f;
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ITakeDamage damage))
            damage.TakeDamage(Damage);
    }
}

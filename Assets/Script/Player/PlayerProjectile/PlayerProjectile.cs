using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class PlayerProjectile : MonoBehaviour
{
    protected Rigidbody _rigidBody;
    protected string _address;
    protected int _damage;
    protected float _timer;
    protected float _flightTime;
    protected float _speed;
    protected bool _isFire;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        _isFire = false;
    }

    protected virtual void OnDisable()
    {
        _rigidBody.velocity = Vector3.zero;
    }

    protected virtual void FixedUpdate()
    {
        if (_isFire)
        {
            if(_timer < _flightTime)
            {
                _timer += Time.fixedDeltaTime;
                Movement();
            }
            else
            {
                ReturnObjectPool();
            }
        }
    }

    protected void ReturnObjectPool()
    {
        _isFire = false;
        ProjectilePool.Instance.EnqueueGameObject(_address, gameObject);
    }

    public virtual void Fire()
    {
        _timer = 0f;
        _isFire = true;
    }

    public void SetData(float flightTime, float speed, int damage)
    {
        _flightTime = flightTime;
        _damage = damage;
        _speed = speed;
    }

    protected virtual void Movement()
    {
        Vector3 direction = transform.forward;
        Vector3 moveVector = direction * _speed;

        _rigidBody.AddForce(moveVector, ForceMode.VelocityChange); //ForceMode.VelocityChange 즉각적인 속도 변화.
    }

    protected virtual void Hit(Collider other)
    {
        if (!other.TryGetComponent(out ITakeDamage iTakeDamage))
            return;

        iTakeDamage.TakeDamage(_damage);
    }
}

using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class PlayerProjectile : MonoBehaviour
{
    protected Rigidbody _rigidBody;
    protected ObjectKey _objectKey;
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
                _isFire = false;
                ReturnObjectPool();
            }
        }
    }

    protected void ReturnObjectPool()
    {
        ObjectPool.Instance.EnqueueGameObject(_objectKey, gameObject);
    }

    public virtual void Fire()
    {
        _timer = 0f;
        _isFire = true;
    }

    public void SetData(float flightTime, float speed, int damage)
    {
        Debug.Log(flightTime);
        Debug.Log(speed);
        Debug.Log(damage);
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

    protected void Hit(Collider other)
    {
        var takeDamage = other.GetComponent<ITakeDamage>();
        if(takeDamage != null)
        {
            takeDamage.TakeDamage(_damage);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Hit(other);
    }
}

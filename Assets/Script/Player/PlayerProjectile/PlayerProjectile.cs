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
    protected float _maxTime = 3f;//임시 코드
    protected float _speed = 5f; //임시 코드
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
            if(_timer < _maxTime)
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

    protected void SetData(float speed, int damage)
    {
        _damage = damage;
        _speed = speed;
    }

    protected virtual void Movement()
    {
        Vector3 direction = transform.forward;
        Vector3 moveVector = direction * _speed;

        _rigidBody.AddForce(moveVector, ForceMode.VelocityChange);
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

using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    protected Rigidbody _rigidBody;
    protected int _damage;
    protected float _timer;
    protected float _maxDistance;
    protected float _speed;
    protected bool _isFire;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        _timer = 0f;
        _isFire = false;
    }

    protected virtual void OnDisable()
    {
        _rigidBody.velocity = Vector3.zero;
    }

    protected virtual void FixedUpdate()
    {
        if(_isFire)
            Movement();
    }

    public virtual void Fire()
    {
        _timer = Time.time;
        _isFire = true;
    }

    protected virtual void SetData(float speed, int damage, float maxDistance)
    {
        _damage = damage;
        _speed = speed;
        _maxDistance = maxDistance;
    }

    protected virtual void Movement()
    {
        Vector3 direction = transform.forward;
        Vector3 moveVector = direction * _speed * Time.fixedDeltaTime;

        _rigidBody.AddForce(moveVector);
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

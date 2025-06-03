using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

public class BombObject : PlayerProjectile
{
    [Header("ParticleSystems")]
    [SerializeField] private ParticleSystem _bomb;
    [SerializeField] private ParticleSystem _explosion;

    [Header("SphereObjectComponent")] 
    [SerializeField] private GameObject _sphereObject;
    private SphereCollider _sphereCollider;
    private LayerMask _targetLayer;
    private float _radius = 3f;

    protected override void Awake()
    {
        base.Awake();

        _sphereCollider = GetComponent<SphereCollider>();
        _objectKey = ObjectKey.PlayerBombPrefab;
        _targetLayer = LayerMask.GetMask("Enemy");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetCollisionEffectActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit(other);
    }

    private void SetCollisionEffectActive(bool enable)
    {
        _sphereObject.SetActive(enable);
        _sphereCollider.enabled = enable;

        if (enable)
            _bomb.Play();
        else
            _bomb.Stop();
    }

    protected override void Hit(Collider other)
    {
        _isFire = false;

        _rigidBody.velocity = Vector3.zero;
        _rigidBody.isKinematic = true;

        StartCoroutine(HitEffect());
        ApplyAreaDamage(other);
    }

    private IEnumerator HitEffect()
    {
        SetCollisionEffectActive(false);

        _explosion.Play();
        yield return new WaitWhile(() => _explosion.isPlaying);

        _rigidBody.isKinematic = false;
        ReturnObjectPool();
    }

    private void ApplyAreaDamage(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,
            _radius, _targetLayer);
        
        if (colliders.Length <= 0)
            return;

        foreach (Collider target in colliders)
        {
            if (target.TryGetComponent(out ITakeDamage iTakeDamage))
                iTakeDamage.TakeDamage(_damage);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class FireBallObject : PlayerProjectile
{
    [Header("ParticleSystem"), SerializeField] private ParticleSystem _particleSystem;
    private int _pierceCount;

    protected override void Awake()
    {
        base.Awake();
        _address = AddressablesKey.Prefab_PlayerFireball;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _pierceCount = 5;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayParticleSystem(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Ignite(other);
        Hit(other);
    }

    public void PlayParticleSystem(bool enable)
    {
        if (enable)
            _particleSystem.Play();
        else
            _particleSystem.Stop();
    }

    protected override void Hit(Collider other)
    {
        base.Hit(other);

        _pierceCount--;
        if (_pierceCount == 0)
            ReturnObjectPool();
    }

    private void Ignite(Collider other)
    {
        if (!other.TryGetComponent(out IBurnable burnable))
            return;

        bool otherIsBurning = burnable.IsBurning();
        if (!otherIsBurning)
            burnable.Ignite();
    }
}

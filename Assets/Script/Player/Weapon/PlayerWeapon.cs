using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour, IWeapon
{
    protected Weapon _weapon;
    protected PlayerAnimationEvent _animEvent;

    protected virtual void Awake()
    {
        _animEvent = GetComponentInChildren<PlayerAnimationEvent>();
    }

    public void InitializeWeapon(Weapon weapon)
    {
        _weapon = weapon;
        _weapon.SetWeaponActive(PlayerHand.Idle);
    }

    public abstract void UseWeapon();

    public Weapon GetWeaponController()
    {
        return _weapon;
    }
}

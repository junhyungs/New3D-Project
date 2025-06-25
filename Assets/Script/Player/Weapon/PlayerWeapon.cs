using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour, IWeapon
{
    protected WeaponObjectController _weaponController;
    protected PlayerWeaponData _data;
    protected PlayerAnimationEvent _animEvent;
    public abstract string AddressableKey { get; }

    protected virtual void Awake()
    {
        _animEvent = GetComponentInChildren<PlayerAnimationEvent>();
    }

    protected void GetWeaponData(DataKey dataKey)
    {
        var key = dataKey.ToString();
        _data = DataManager.Instance.GetData(key) as PlayerWeaponData;
    }

    public void InitializeWeapon(WeaponObjectController weapon)
    {
        _weaponController = weapon;
        _weaponController.SetWeaponActive(PlayerHand.Idle);

        _animEvent.SetWeaponAction(UseWeapon);
    }

    public abstract void UseWeapon();

    public PlayerWeaponData GetWeaponData()
    {
        return _data;
    }

    public WeaponObjectController GetWeaponController()
    {
        return _weaponController;
    }

    protected void FindTarget()
    {
        var targetLayer = LayerMask.GetMask("Enemy");
        var boxPosition = transform.position + transform.forward + Vector3.up * 0.6f;
        var boxSize = _data.Range;

        Collider[] colliders = Physics.OverlapBox(boxPosition, boxSize / 2f, transform.rotation, targetLayer);
        var damage = _data.Damage;
        foreach(var target in colliders)
            HitTarget(target, damage);
    }

    private void HitTarget(Collider target, int damage)
    {
        TakeDamage(target, damage);
        Interact(target);
    }

    private void TakeDamage(Collider target, int damage)
    {
        if (!target.TryGetComponent(out ITakeDamage iTakeDamage))
            return;

        iTakeDamage.TakeDamage(damage);
    }

    private void Interact(Collider target)
    {
        if(!target.TryGetComponent(out IInteractionGameObject iInteractionGameObject))
            return;

        bool isWeaponInteractable = iInteractionGameObject.IsWeaponInteractable;
        if (isWeaponInteractable)
        {
            iInteractionGameObject.Interact();
        }
    }
}

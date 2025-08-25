using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyComponent;

public class SlimeWeaponController : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Transform _parent;

    private Slime _owner;
    private SlimeWeapon[] _weapons;
    private const int MAXCOUNT = 8;

    private void Awake()
    {
        _owner = GetComponent<Slime>();
    }

    private void Start()
    {
        CreateWeapon();
    }

    private void CreateWeapon()
    {
        if (_weaponObject == null)
            return;

        _weapons = new SlimeWeapon[MAXCOUNT];
        var y = 0f;
        for(int i = 0; i < MAXCOUNT; i++)
        {
            var weaponObject = Instantiate(_weaponObject, _parent);
            weaponObject.transform.localRotation = Quaternion.Euler(0f, y, 90f);

            var weaponComponent = weaponObject.GetComponent<SlimeWeapon>();
            if(weaponComponent != null)
                _weapons[i] = weaponComponent;

            y += 45f;
        }
    }

    public void Attack()
    {
        foreach (var weapon in _weapons)
        {
            if(_owner.Property != null)
            {
                var data = _owner.Property.Data;
                weapon.Damage = data.Damage;
            }
            
            weapon.StartMoveCoroutine();
        }
    }
}

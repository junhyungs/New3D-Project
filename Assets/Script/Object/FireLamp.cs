using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLamp : MonoBehaviour, IBurnable
{
    [Header("IsBurning"), SerializeField] private bool _isBurning;
    [Header("FireParticleObject"), SerializeField] private GameObject _fireParticleObject;

    private void Start()
    {
        if (_isBurning)
            _fireParticleObject.SetActive(true);
    }

    public bool IsBurning()
    {
        return _isBurning;
    }

    public void Ignite()
    {
        _isBurning = true;
        _fireParticleObject.SetActive(true);
    }
}

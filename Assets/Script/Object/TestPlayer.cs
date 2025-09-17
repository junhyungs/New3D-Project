using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour, ITakeDamage
{
    [Header("Health")]
    [SerializeField] private int _health;
    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log(_health);
    }
}

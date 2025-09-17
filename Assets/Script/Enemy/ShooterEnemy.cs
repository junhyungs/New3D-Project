using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public abstract class ShooterEnemy<TProperty, TShooter> : Enemy<TProperty>
        where TProperty : IPropertyBase
        where TShooter : EnemyProjectileShooter
    {
        protected TShooter _shooter;
        protected override void OnAwakeEnemy()
        {
            base.OnAwakeEnemy();
            _shooter = GetComponent<TShooter>();
            if (_shooter != null)
                _shooter.Damage = GetDamage();
        }

        /// <summary>
        /// OnAwake
        /// </summary>
        /// <returns>Data.Damage</returns>
        protected abstract int GetDamage();
    }
}


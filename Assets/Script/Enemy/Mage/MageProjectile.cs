using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class MageProjectile : EnemyProjectile
    {
        public override void SetupProjectile(int damage, Vector3 direction)
        {
            transform.parent = null;

            _direction = direction;
            _isMove = true;
            _damage = damage;
        }

        protected override void OnUpdateProjectile()
        {
            Vector3 moveVector = _direction * _dataSO.Speed * Time.deltaTime;
            transform.Translate(moveVector, Space.World);
        }
    }
}


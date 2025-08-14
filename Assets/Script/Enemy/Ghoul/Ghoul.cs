using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class Ghoul : ShooterEnemy<GhoulProperty, GhoulShooter>
    {
        [Header("TestData")] //테스트 코드
        public GhoulSO _testData;

        protected override GhoulProperty CreateProperty()
        {
            return new GhoulProperty(this);
        }

        protected override void Death()
        {
            _shooter.AllDisableProjectile();
            Property.StateMachine.ChangeState(E_GhoulState.Death);
        }

        protected override void OnStartEnemy()
        {
            StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            yield return new WaitForSeconds(5f);
            TakeDamage(_testData.Health);
        }

        protected override int GetDamage()
        {
            return Property.Data.Damage;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            var radius = Property.Data.DetectionRange;
            Gizmos.DrawWireSphere(transform.position, radius);

            Gizmos.color = Color.white;
            radius = Property.Data.AgentStopDistance;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}


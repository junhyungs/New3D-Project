using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class Bat : Enemy<BatProperty>
    {
        [Header("TestData")]
        public BatSO _batSO;

        protected override BatProperty CreateProperty()
        {
            return new BatProperty(this);
        }

        protected override void Death()
        {
            Property.StateMachine.ChangeState(E_BatState.Death);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
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


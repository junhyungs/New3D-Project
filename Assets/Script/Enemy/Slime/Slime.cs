using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

namespace EnemyComponent
{
    public class Slime : Enemy<SlimeProperty>
    {
        [Header("TestData")]
        public SlimeSO _slimeSO;

        protected override SlimeProperty CreateProperty()
        {
            return new SlimeProperty(this);
        }

        protected override void Death()
        {
            Property.StateMachine.ChangeState(E_SlimeState.Death);
        }

        public void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Property.Data.DetectionRange);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, Property.Data.AgentStopDistance);
        }
    }
}


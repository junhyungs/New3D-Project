using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyComponent
{
    public class MageTeleport : MageState, ICharacterState<MageTeleport>
    {
        public MageTeleport(Mage mage) : base(mage)
        {
            _agent = mage.GetComponent<NavMeshAgent>();
        }

        private NavMeshAgent _agent;

        public void OnStateEnter()
        {

        }

        //private IEnumerator Teleport()
        //{

        //}
    }
}


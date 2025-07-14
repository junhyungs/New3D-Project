using EnumCollection;
using State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class MageStateMachine : EnemyStateMachine<Mage, MageStateFactory, E_MageState>
    {
        protected override E_MageState GetInitializeState()
        {
            return E_MageState.Idle;
        }
    }
}


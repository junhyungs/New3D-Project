using EnumCollection;
using State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class ForestMotherStateMachine : EnemyStateMachine<ForestMother, ForestMotherFactory, E_ForestMotherState>
    {
        protected override E_ForestMotherState GetInitializeState()
        {
            return E_ForestMotherState.ChangePattern;
        }
    }
}


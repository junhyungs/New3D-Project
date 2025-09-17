using EnumCollection;
using State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class SlimeStateMachine : EnemyStateMachine<Slime, SlimeFactory, E_SlimeState>
    {
        protected override E_SlimeState GetInitializeState()
        {
            return E_SlimeState.Patrol;
        }

        protected override void OnEnableStateMachine()
        {
            var stateDic = _stateMachine.StateDictionary;
            foreach (var state in stateDic.Values)
                if (state is IInitializeEnable init)
                    init.Init();
        }
    }
}


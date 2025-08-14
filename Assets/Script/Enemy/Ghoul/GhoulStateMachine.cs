using EnumCollection;
using State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class GhoulStateMachine : EnemyStateMachine<Ghoul, GhoulStateFactory, E_GhoulState>
    {
        protected override E_GhoulState GetInitializeState()
        {
            return E_GhoulState.Partrol;
        }

        protected override void OnEnableStateMachine()
        {
            if (_stateMachine == null)
                return;

            var stateDictionary = _stateMachine.StateDictionary;
            foreach (var state in stateDictionary.Values)
                if (state is IInitializeEnable init)
                    init.Init();

            _stateMachine.StartState(GetInitializeState());
        }
    }
}


using EnumCollection;
using State;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class BatStateMachine : EnemyStateMachine<Bat, BatStateFactory, E_BatState>
    {
        protected override E_BatState GetInitializeState()
        {
            return E_BatState.Patrol;
        }

        protected override void OnEnableStateMachine()
        {
            ForeachState<IInitializeEnable>((init) => init.Init());
        }

        protected override void OnDestroyStateMachine()
        {
            ForeachState<IUnbindAction>((unbind) => unbind.Unbind());
        }

        private void ForeachState<T>(Action<T> action)
        {
            var stateDictionary = _stateMachine.StateDictionary;
            foreach(var state in stateDictionary.Values)
                if(state is T tType)
                    action(tType);
        }
    }
}


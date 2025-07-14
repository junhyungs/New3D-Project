using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using EnumCollection;
using System;

namespace EnemyComponent
{
    public abstract class EnemyStateMachine<TClass, TFactory, TEnum> : MonoBehaviour, IGetState<TEnum>
        where TClass : class
        where TEnum : Enum
        where TFactory : ICharacterStateFactory<TClass, TEnum>, new()
    {
        private CharacterStateMachine<TClass, TEnum, TFactory> _stateMachine;

        protected virtual void Start()
        {
            InitializeOnStart();
        }

        protected virtual void InitializeOnStart()
        {
            var referenceClass = GetComponent<TClass>();

            _stateMachine = new CharacterStateMachine<TClass, TEnum, TFactory>();
            _stateMachine.CreateState(referenceClass);
            _stateMachine.StartState(GetInitializeState());
        }

        protected abstract TEnum GetInitializeState();

        protected virtual void Update()
        {
            _stateMachine.Update();
        }

        protected void ChangeState(TEnum nextState)
        {
            _stateMachine.ChangeState(nextState);
        }

        public ICharacterState GetCurrentState()
        {
            return _stateMachine.GetCurrentState();
        }

        public ICharacterState GetState(TEnum stateName)
        {
            return _stateMachine.GetState(stateName);
        }

        public TEnum GetCurrentStateType()
        {
            return _stateMachine.GetCurrentStateType();
        }
    }
}


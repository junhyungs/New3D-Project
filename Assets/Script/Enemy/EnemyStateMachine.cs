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

        private void OnEnable()
        {
            OnEnableStateMachine();
        }

        private void Start()
        {
            OnStartStateMachine();
        }

        protected virtual void OnEnableStateMachine()
        {
            if(_stateMachine != null)
                _stateMachine.StartState(GetInitializeState());
        }

        public virtual void OnStartStateMachine()
        {
            var myClass = GetComponent<TClass>();
            if (myClass == null)
                return;

            _stateMachine = new CharacterStateMachine<TClass, TEnum, TFactory>();
            _stateMachine.CreateState(myClass);
            _stateMachine.StartState(GetInitializeState());
        }

        protected abstract TEnum GetInitializeState();

        protected virtual void Update()
        {
            _stateMachine.Update();
        }

        public void ChangeState(TEnum nextState)
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


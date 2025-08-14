using EnumCollection;
using State;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyComponent
{
    
    public abstract class EnemyStateMachine<TClass, TFactory, TEnum> : MonoBehaviour, IStateController<TEnum>
        where TClass : class
        where TEnum : Enum
        where TFactory : ICharacterStateFactory<TClass, TEnum>, new()
    {
        protected CharacterStateMachine<TClass, TEnum, TFactory> _stateMachine;

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

        public void ChangeState(TEnum state)
        {
            _stateMachine.ChangeState(state);
        }
    }
}


using EnumCollection;
using State;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
            if (_stateMachine == null)
                return;

            InitState();
            OnEnableStateMachine();
        }

        private void Start()
        {
            CreateState();
            OnStartStateMachine();
        }

        private void OnDestroy()
        {
            OnDestroyStateMachine();
        }

        protected virtual void OnEnableStateMachine() { }
        public virtual void OnStartStateMachine() { }
        protected virtual void OnDestroyStateMachine() { }

        private void CreateState()
        {
            var myClass = GetComponent<TClass>();
            if (myClass == null)
                return;

            _stateMachine = new CharacterStateMachine<TClass, TEnum, TFactory>();
            _stateMachine.CreateState(myClass);
            AwakeState();
            InitState();
        }

        private void InitState() =>
            _stateMachine.StartState(GetInitializeState());

        private void AwakeState()
        {
            var stateDictionary = _stateMachine.StateDictionary;
            foreach (var state in stateDictionary.Values)
                if (state != null)
                    state.AwakeState();
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


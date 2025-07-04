using System;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class CharacterStateMachine<TClass, TEnum, TFactroy>
        where TClass : class
        where TEnum : Enum
        where TFactroy : ICharacterStateFactory<TClass, TEnum>, new()
    {
        public CharacterStateMachine()
        {
            _factory = new CharacterStateFactory<TClass, TEnum, TFactroy>();
        }

        private Dictionary<TEnum, ICharacterState> _stateDictionary;
        private CharacterStateFactory<TClass, TEnum, TFactroy> _factory;
        private ICharacterState _state;
        private TEnum _currentType;

        public Dictionary<TEnum, ICharacterState> StateDictionary => _stateDictionary;

        public void FixedUpdate()
        {
            _state.OnStateFixedUpdate();
        }

        public void Update()
        {
            _state.OnStateUpdate();
        }

        public void OnTriggerEnter(Collider other)
        {
            _state.OnTriggerEnter(other);
        }

        public void StartState(TEnum enumType)
        {
            if (_stateDictionary.ContainsKey(enumType))
            {
                _state = _stateDictionary[enumType];
                _state.OnStateEnter();
            }
        }

        public void ChangeState(TEnum newState)
        {
            if (_stateDictionary.ContainsKey(newState))
            {
                _state.OnStateExit();
                _state = _stateDictionary[newState];
                _currentType = newState;
                _state.OnStateEnter();
            }
        }

        public void CreateState(TClass classType)
        {
            _stateDictionary = _factory.CreateState(classType);
        }

        public ICharacterState GetCurrentState()
        {
            return _state;
        }

        public ICharacterState GetState(TEnum enumType)
        {
            if(_stateDictionary.ContainsKey(enumType))
                return _stateDictionary[enumType];

            return null;
        }

        public TEnum GetCurrentStateType()
        {
            return _currentType;
        }
    }   
}


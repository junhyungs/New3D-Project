using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using State;

namespace PlayerComponent
{
    public class PlayerStateMachine : MonoBehaviour, IStateController<E_PlayerState>
    {
        private CharacterStateMachine<Player, E_PlayerState, PlayerStateFactory> _stateMachine;

        private void OnEnable()
        {
            if (_stateMachine == null)
                return;

            var stateDic = _stateMachine.StateDictionary;
            foreach(var state in stateDic.Values)
            {
                if (state is IInitializeEnable init)
                    init.Init();
            }
        }

        private void Start()
        {
            InitializeOnStart();
        }

        private void InitializeOnStart()
        {
            var player = GetComponent<Player>();
            _stateMachine = new CharacterStateMachine<Player, E_PlayerState, PlayerStateFactory>();

            _stateMachine.CreateState(player);
            _stateMachine.StartState(E_PlayerState.Idle);
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public void ChangeState(E_PlayerState nextState)
        {
            _stateMachine.ChangeState(nextState);
        }

        public ICharacterState GetCurrentState()
        {
            return _stateMachine.GetCurrentState(); 
        }

        public E_PlayerState GetCurrentStateType()
        {
            return _stateMachine.GetCurrentStateType();
        }

        public ICharacterState GetState(E_PlayerState stateName)
        {
            return _stateMachine.GetState(stateName);
        }
    }
}


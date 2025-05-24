using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using State;

namespace PlayerComponent
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private CharacterStateMachine<Player, E_PlayerState, PlayerStateFactory> _stateMachine;

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

        public T GetState<T>(E_PlayerState stateName) where T : PlayerState
        {
            var state = _stateMachine.GetState(stateName);
            return state is T targetState ? targetState : null;
        }
    }
}


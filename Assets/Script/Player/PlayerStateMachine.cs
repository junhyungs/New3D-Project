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

        private void Awake()
        {
            InitializeOnAwake();
        }

        private void InitializeOnAwake()
        {
            var player = GetComponent<Player>();

            _stateMachine = new CharacterStateMachine<Player, E_PlayerState, PlayerStateFactory>();
            _stateMachine.CreateState(player);
        }

        private void Start()
        {
            _stateMachine.StartState(E_PlayerState.Idle);
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        public void ChangeState(E_PlayerState nextState)
        {
            _stateMachine.ChangeState(nextState);
        }
    }
}


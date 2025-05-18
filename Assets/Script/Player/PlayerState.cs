using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerComponent
{
    public class PlayerState
    {
        public PlayerState(Player player)
        {
            _player = player;
            _stateMachine = player.GetComponent<PlayerStateMachine>();

            InitializeInputHandler(player);
        }

        protected Player _player;
        protected PlayerStateMachine _stateMachine;
        protected PlayerInputHandler _inputHandler;

        private void InitializeInputHandler(Player player)
        {
            var playerInput = player.GetComponent<PlayerInput>();

            _inputHandler = new PlayerInputHandler(playerInput);
        }

        protected virtual void InputCheck() { }
    }
}


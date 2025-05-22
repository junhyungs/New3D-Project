using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Idle : PlayerState, ICharacterState<Idle>
    {
        public Idle(Player player) : base(player) { }
        
        public void OnStateEnter() { }

        public void OnStateFixedUpdate()
        {
            InputCheck();
        }

        public void OnStateExit() { }

        protected override void InputCheck()
        {
            var vector2 = _inputHandler.MoveVector;

            if (vector2 != Vector2.zero)
                _stateMachine.ChangeState(E_PlayerState.Move);
        }
    }
}


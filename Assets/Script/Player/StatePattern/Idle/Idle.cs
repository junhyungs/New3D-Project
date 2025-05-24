using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Idle : PlayerMoveState, ICharacterState<Idle>
    {
        public Idle(Player player) : base(player) { }

        public void OnStateFixedUpdate()
        {
            IsFalling(E_PlayerState.Falling);
            _stateHandler.IdleToMoveState();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Idle : PlayerState, ICharacterState<Idle>
    {
        public Idle(Player player) : base(player) { }
       
        public void OnStateFixedUpdate()
        {
            _stateHandler.IdleToMoveState();
        }
    }
}


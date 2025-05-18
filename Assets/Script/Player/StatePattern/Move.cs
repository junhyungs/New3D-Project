using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Move : PlayerMoveState, ICharacterState<Move>
    {
        public Move(Player player) : base(player) { }

        public void OnStateEnter()
        {

        }

        public void OnStateFixedUpdate()
        {

        }

        public void OnStateExit()
        {

        }

        protected override void InputCheck()
        {
            
        }
    }

}

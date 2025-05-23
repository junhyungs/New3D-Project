using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Climbing : PlayerMoveState, ICharacterState<Climbing>
    {
        public Climbing(Player player) : base(player)
        {

        }

        public void OnStateEnter()
        {

        }

        public void SetLadderSize((float, float) ladderSize) { }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class RollSlash : PlayerRollState<RollSlashStateBehaviour>, ICharacterState<RollSlash>
    {
        public RollSlash(Player player) : base(player)
        {
            _behaviour.RollSlashState = this;
        }

        public bool AnimationStop { get; set; }

        public void OnStateEnter()
        {
            TriggerAnimation(_rollSlash);
        }

        public void OnStateFixedUpdate()
        {
            if (IsRoll)
            {
                Movement();
            }

            if (AnimationStop)
                ChangeState();
        }
    }
}


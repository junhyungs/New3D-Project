using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(Player player) : base(player)
        {
            Initialize(player);
        }

        protected Animator _animator;

        private void Initialize(Player player)
        {
            _animator = player.GetComponentInChildren<Animator>();
        }
    }
}


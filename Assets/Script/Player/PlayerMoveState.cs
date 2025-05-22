using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Player player) : base(player)
        {
            Initialize(player);
        }

        protected Animator _animator;
        protected Rigidbody _rigidBody;
        protected Transform _playerTransform;

        private void Initialize(Player player)
        {
            _animator = player.GetComponent<Animator>();
            _rigidBody = player.GetComponent<Rigidbody>();
            _playerTransform = player.GetComponent<Transform>();
        }
    }
}


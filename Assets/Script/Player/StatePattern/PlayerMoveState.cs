using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

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

        protected const float RAYDISTANCE = 1.5f;
        protected float _checkSphereRadius = 1f;
        protected LayerMask _ground = LayerMask.GetMask("Ground");

        private void Initialize(Player player)
        {
            _animator = player.GetComponentInChildren<Animator>();
            _rigidBody = player.GetComponent<Rigidbody>();
            _playerTransform = player.GetComponent<Transform>();
        }

        protected virtual void CheckGround() { }
    }
}


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

        protected float _checkSphereRadius = 0.5f;
        protected LayerMask _ground = LayerMask.GetMask("Ground");

        private void Initialize(Player player)
        {
            _animator = player.GetComponentInChildren<Animator>();
            _rigidBody = player.GetComponent<Rigidbody>();
            _playerTransform = player.GetComponent<Transform>();
        }

        protected void IsFalling(E_PlayerState state)
        {
            bool isGround = Physics.CheckSphere(_playerTransform.position, _checkSphereRadius, _ground);

            if (state == E_PlayerState.Falling && !isGround)
                _stateMachine.ChangeState(E_PlayerState.Falling);
            else if (state == E_PlayerState.Idle && isGround)
                _stateMachine.ChangeState(E_PlayerState.Idle);
        }
    }
}


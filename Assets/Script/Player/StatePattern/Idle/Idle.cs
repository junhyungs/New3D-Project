using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Idle : PlayerMoveState, ICharacterState<Idle>
    {
        public Idle(Player player) : base(player) { }

        private readonly int _moveValue = Animator.StringToHash("MoveValue");

        public void OnStateEnter()
        {
            CheckBlendTree();
        }

        private void CheckBlendTree()
        {
            if(_animator.GetFloat(_moveValue) != 0f)
                _animator.SetFloat(_moveValue, 0f);
        }

        public void OnStateFixedUpdate()
        {
            IsFalling(E_PlayerState.Falling);
            _stateHandler.IdleToMoveState();
        }
    }
}


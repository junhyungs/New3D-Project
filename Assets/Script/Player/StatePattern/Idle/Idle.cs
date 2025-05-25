using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Idle : PlayerMoveState, ICharacterState<Idle>
    {
        public Idle(Player player) : base(player) { }

        public void OnStateEnter()
        {
            CheckBlendTree();
        }

        private void CheckBlendTree()
        {
            var blendValue = _animator.GetFloat("MoveValue");
            if (blendValue != 0)
                _animator.SetFloat("MoveValue", 0f);
        }

        public void OnStateFixedUpdate()
        {
            IsFalling(E_PlayerState.Falling);
            _stateHandler.IdleToMoveState();
        }
    }
}


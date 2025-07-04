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
            CheckGround();
            _stateHandler.ChangeIdleORMoveState();
        }

        protected override void CheckGround()
        {
            var origin = _playerTransform.position + Vector3.up * 0.1f;

            bool isGround = Physics.Raycast(origin, Vector3.down, RAYDISTANCE, _ground);
            if (!isGround)
                _stateHandler.ChangeState(E_PlayerState.Falling);
        }
    }
}


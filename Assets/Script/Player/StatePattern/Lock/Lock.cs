using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Lock : PlayerMoveState, ICharacterState<Lock>
    {
        public Lock(Player player) : base(player) { }
        private readonly int _moveValue = Animator.StringToHash("MoveValue");
        public void OnStateEnter()
        {
            _animator.SetFloat(_moveValue, 0);
            _inputHandler.LockPlayer(false);
        }

        public void OnStateExit()
        {
            _inputHandler.LockPlayer(true);
        }
    }
}


using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerRollState<T> : PlayerMoveState where T : StateMachineBehaviour
    { 
        public PlayerRollState(Player player) : base(player)
        {
            _behaviour = _animator.GetBehaviour<T>();
        }

        protected T _behaviour;
        protected readonly int _roll = Animator.StringToHash("IsRoll");
        protected readonly int _rollSlash = Animator.StringToHash("IsRollSlash");

        protected Vector3 _direction;

        public bool IsRoll { get; set; }

        protected void Movement()
        {
            Vector3 rollVector = _direction * _data.RollSpeed * Time.fixedDeltaTime;
            _rigidBody.MovePosition(_rigidBody.position + rollVector);
        }

        protected void TriggerAnimation(int parameter)
        {
            _direction = _playerTransform.forward.normalized;
            _animator.SetTrigger(parameter);
        }

        protected void ChangeState()
        {
            var nextState = _stateHandler.MoveVector != Vector2.zero ? E_PlayerState.Move : E_PlayerState.Idle;
            _stateHandler.ChangeState(nextState);
        }
    }
}


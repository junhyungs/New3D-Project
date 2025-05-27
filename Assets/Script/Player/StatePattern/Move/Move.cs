using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Move : PlayerMoveState, ICharacterState<Move>
    {
        public Move(Player player) : base(player)
        {
            _speedChangeValue = _constantData.SpeedChangeValue;
        }

        private readonly int _moveValue = Animator.StringToHash("MoveValue");

        private float _targetSpeed;
        private float _currentSpeed;
        private float _speedChangeValue;
        private float _targetRotation;
        private float _rotationVelocity;

        public void OnStateFixedUpdate()
        {
            IsFalling(E_PlayerState.Falling);
            Movement();
        }

        public void OnStateExit()
        {
            _rigidBody.velocity = Vector3.zero;
        }

        private void Movement()
        {
            _targetSpeed = _data.Speed;

            if(_stateHandler.MoveVector == Vector2.zero)
            {
                _targetSpeed = 0f;
                _speedChangeValue = 5f;

                if(_currentSpeed == _targetSpeed)
                {
                    _stateHandler.ChangeIdleORMoveState();
                    return;
                }
            }
            else
            {
                if(_speedChangeValue != _constantData.SpeedChangeValue)
                {
                    _speedChangeValue = Mathf.Lerp(_speedChangeValue, _constantData.SpeedChangeValue, Time.fixedDeltaTime);
                }
            }

                var currentHorizontalSpeed = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z).magnitude;
            bool isChange = currentHorizontalSpeed < _targetSpeed - _constantData.SpeedOffSet 
                || currentHorizontalSpeed > _targetSpeed + _constantData.SpeedOffSet;

            if (isChange)
            {
                _currentSpeed = Mathf.Lerp(currentHorizontalSpeed, _targetSpeed, _speedChangeValue * Time.fixedDeltaTime);
                _currentSpeed = Mathf.Round(_currentSpeed * 1000f) / 1000f;
            }
            else
                _currentSpeed = _targetSpeed;

            Vector3 direction = new Vector3(_stateHandler.MoveVector.x, 0f, _stateHandler.MoveVector.y).normalized;
            if(direction != Vector3.zero)
            {
                _targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                var smoothAngle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, _targetRotation, ref _rotationVelocity, 0.12f);
                _playerTransform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }

            Vector3 moveVector = direction * _currentSpeed;
            moveVector.y = _rigidBody.velocity.y;
            _rigidBody.velocity = moveVector;

            var normalizedSpeed = _currentSpeed / _data.Speed;
            _animator.SetFloat(_moveValue, normalizedSpeed);
        }
    }

}

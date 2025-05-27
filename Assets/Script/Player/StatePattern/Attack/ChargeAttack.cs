using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace PlayerComponent
{
    public class ChargeAttack : PlayerAttackState, ICharacterState<ChargeAttack>
    {
        public ChargeAttack(Player player) : base(player)
        {
        }

        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

        private readonly int _chargeAttack = Animator.StringToHash("ChargeAttack");
        private readonly int _chargeEquals = Animator.StringToHash("ChargeEquals");

        private const string _charge_max_L = "Charge_max_L";
        private const string _charge_max_R = "Charge_max_R";
        private const string _first_Slash = "First_Slash";
        private const string _second_Slash = "Second_Slash";

        public bool Pressed { get; set; }
        private bool _attackDirection = true;

        private const float _maxDistance = 5f;
        private float _speed;
        private float _moveDistance;

        private Vector3 _startPos;
        private Vector3 _currentPos;

        public void OnStateEnter()
        {
            _animator.SetBool(_chargeAttack, Pressed);

            var equals = Convert.ToInt32(_attackDirection);
            _animator.SetInteger(_chargeEquals, equals);
        }

        public void OnStateUpdate()
        {
            if (!Pressed)
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                bool isMax = stateInfo.IsName(_charge_max_L)
                    || stateInfo.IsName(_charge_max_R);

                if (isMax)
                {
                    LookAtCursor();
                    _player.StartCoroutine(DashMovement());
                }

                _animator.SetBool(_chargeAttack , Pressed);
            }
        }

        private IEnumerator DashMovement()
        {
            yield return new WaitUntil(() =>
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName(_first_Slash) || stateInfo.IsName(_second_Slash);
            });

            _startPos = _rigidbody.position;
           
            while (!_animator.IsInTransition(0))
            {
                _currentPos = _rigidbody.position;
                _moveDistance = Vector3.Distance(_startPos, _currentPos);

                if (_moveDistance > _maxDistance)
                    break;

                _speed = (_constantData.DashSpeed * 10f) * Time.fixedDeltaTime;
                Vector3 moveVector = _playerTransform.forward * _speed;

                _rigidbody.MovePosition(_rigidbody.position + moveVector);
                yield return _waitForFixedUpdate;
            }

            _attackDirection = !_attackDirection;
            _stateHandler.ChangeIdleORMoveState();
        }
    }
}


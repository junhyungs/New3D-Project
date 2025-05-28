using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace PlayerComponent
{
    public class ChargeAttack : PlayerAttackState, ICharacterState<ChargeAttack>
    {
        public ChargeAttack(Player player) : base(player) { }
        private WaitForFixedUpdate _waitForFixedUpdate;

        private readonly int _chargeAttack = Animator.StringToHash("ChargeAttack");
        private readonly int _chargeEquals = Animator.StringToHash("ChargeEquals");

        private const string _charge_max_L = "Charge_max_L";
        private const string _charge_max_R = "Charge_max_R";
        private const string _first_Slash = "First_Slash";
        private const string _second_Slash = "Second_Slash";

        public bool Pressed { get; set; }
        private bool _attackDirection = true;

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
                    _monobehaviour.StartCoroutine(DashMovement());
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

            var speed = _constantData.DashSpeed * 2;
            var maxDistance = 5f;
            var startPosition = _rigidbody.position;

            while (!_animator.IsInTransition(0))//상태전환이 일어나기 전까지 반복.
            {
                var currentPosition = _rigidbody.position;
                var moveDistance = Vector3.Distance(startPosition, currentPosition);

                if (moveDistance > maxDistance)
                    break;

                var dashSpeed = speed * Time.fixedDeltaTime;
                var moveVector = _playerTransform.forward * dashSpeed;

                _rigidbody.MovePosition(_rigidbody.position + moveVector);
                yield return _waitForFixedUpdate;
            }

            _attackDirection = !_attackDirection;
            _stateHandler.ChangeIdleORMoveState();
        }
    }
}


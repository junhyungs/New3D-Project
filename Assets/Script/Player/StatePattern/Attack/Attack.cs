using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Attack : PlayerAttackState, ICharacterState<Attack>, IAttackStateEventReceiver
    {
        public Attack(Player player) : base(player)
        {
            var stateBehaviour = _animator.GetBehaviour<AttackStateBehaviour>();
            stateBehaviour.IAttack = this;
        }
        
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly int _isAttack = Animator.StringToHash("IsAttack");
        private int _comboCount;

        public float NextCombo { get; set; } = 0.4f;
        private float _lastClickTime;
        private float _changeTime;
        private float _moveTimer;

        private bool _animationMovement;
        private bool _isNextClick;

        private Vector3 _movePos;

        public void OnStateEnter()
        {
            _animator.SetTrigger(_attack);
            _animator.SetBool(_isAttack, true);

            LookAtCursor();
            _lastClickTime = Time.time;
        }

        public void OnStateFixedUpdate()
        {
            if (!_animationMovement)
                return;

            _moveTimer += Time.fixedDeltaTime;
            if (_moveTimer >= 0.2f)
                return;

            var moveVector = _movePos * _constantData.DashSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + moveVector);
        }

        public void OnStateUpdate()
        {
            if (_isNextClick)
            {
                bool nextCombo = Time.time - _lastClickTime > NextCombo && _comboCount < 3;
                if (nextCombo)
                {
                    _animator.SetTrigger(_attack);

                    LookAtCursor();

                    _lastClickTime = Time.time;
                    _isNextClick = false;
                    _changeTime = 0f;

                    return;
                }
            }

            _changeTime += Time.deltaTime;
            if (_changeTime >= 0.63f)
                _stateHandler.ChangeIdleORMoveState();
        }

        public void OnStateExit()
        {
            _animator.SetBool(_isAttack, false);
            _lastClickTime = 0f;
            _changeTime = 0f;
            _comboCount = 0;
        }

        public void SetClick(bool isClick)
        {
            _isNextClick = isClick; //상태 진입 이후, 추가적인 클릭여부를 받는 메서드.
        }

        public void OnAttackAnimEnter()
        {
            _movePos = _playerTransform.forward;
            _animationMovement = true;
            _moveTimer = 0f;
            _comboCount++;
        }

        public void OnAttackAnimExit()
        {
            _animationMovement = false;
        }
    }

    public interface IAttackStateEventReceiver
    {
        public void OnAttackAnimEnter();
        public void OnAttackAnimExit();
    }
}


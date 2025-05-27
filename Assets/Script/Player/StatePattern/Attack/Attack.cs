using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Attack : PlayerAttackState, ICharacterState<Attack>
    {
        public Attack(Player player) : base(player) { }
        
        private readonly int _attack = Animator.StringToHash("Attack");
        public float NextCombo { get; set; } = 0.4f;

        private float _lastClickTime;
        private float _changeTime;

        private bool _isClick;

        public void OnStateEnter()
        {
            ClickTriggerAttack();
            _lastClickTime = Time.time;
        }

        public void OnStateUpdate()
        {
            ComboAttack();
        }

        public void OnStateExit()
        {
            _lastClickTime = 0f;
            _changeTime = 0f;
        }

        private void ComboAttack()
        {
            if (_isClick)
            {
                if (Time.time - _lastClickTime >= NextCombo)
                {
                    ClickTriggerAttack();

                    _lastClickTime = Time.time;
                    _isClick = false;
                }

                _changeTime = 0f;
            }
            else
            {
                _changeTime += Time.deltaTime;
                
                if(_changeTime > 0.65f)
                {
                    _stateHandler.ChangeIdleORMoveState();
                }
            }
        }

        public void SetClick(bool isClick)
        {
            _isClick = isClick;
        }

        private void ClickTriggerAttack()
        {
            LookAtCursor();
            _animator.SetTrigger(_attack);
        }
    }
}


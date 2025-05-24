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
            _lastClickTime = Time.time;
            _animator.SetTrigger(_attack);
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
                    _animator.SetTrigger(_attack);
         
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
                    _stateHandler.ChangeState(E_PlayerState.Idle);
                }
            }
        }

        public void SetClick(bool isClick)
        {
            _isClick = isClick;
        }
    }
}


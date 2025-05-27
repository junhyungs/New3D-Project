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

        private readonly int _chargeAttack = Animator.StringToHash("ChargeAttack");
        private readonly int _chargeEquals = Animator.StringToHash("ChargeEquals");

        private const string _charge_max_L = "Charge_max_L";
        private const string _charge_max_R = "Charge_max_R";

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
                    Debug.Log("ChargeAttack");
                    _attackDirection = !_attackDirection;
                }

                _animator.SetBool(_chargeAttack , Pressed);
                _stateHandler.ChangeIdleORMoveState();
            }
        }
    }
}


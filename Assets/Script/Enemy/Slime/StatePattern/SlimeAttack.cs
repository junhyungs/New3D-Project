using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class SlimeAttack : SlimeState, ICharacterState<SlimeAttack>
    {
        public SlimeAttack(Slime owner) : base(owner) { }

        private readonly int _attack = Animator.StringToHash("Attack");

        public void OnStateEnter()
        {
            _property.Animator.SetTrigger(_attack);
        }

        public void OnStateUpdate()
        {
            var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsTag("Attack"))
                return;

            if (stateInfo.normalizedTime >= 0.98f)
                _property.StateMachine.ChangeState(E_SlimeState.CoolDown);
        }

    }
}

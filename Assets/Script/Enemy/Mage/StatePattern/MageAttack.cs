using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class MageAttack : MageState, ICharacterState<MageAttack>
    {
        public MageAttack(Mage mage) : base(mage) { }

        private readonly int _attack = Animator.StringToHash("Attack");

        public void OnStateEnter()
        {
            _targetTransform = FindTarget();
            _property.Owner.StartCoroutine(StartAttack());
        }

        private IEnumerator StartAttack()
        {
            _property.Animator.SetTrigger(_attack);

            yield return null;
            yield return new WaitUntil(() =>
            {
                var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsTag("Mage_Attack") && stateInfo.normalizedTime > 0.8f;
            });

            yield return new WaitForSeconds(1f);
            _property.StateMachine.ChangeState(E_MageState.Teleport);
        }
    }
}


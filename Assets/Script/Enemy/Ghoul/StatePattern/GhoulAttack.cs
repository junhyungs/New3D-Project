using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
namespace EnemyComponent
{
    public class GhoulAttack : GhoulState, ICharacterState<GhoulAttack>, IInitializeEnable
    {
        public GhoulAttack(Ghoul owner) : base(owner)
        {
            GetBehaviour();
        }

        private Transform _targetTransform;
        private readonly int _attack = Animator.StringToHash("Attack");

        public void OnStateEnter()
        {
            _targetTransform = FindPlayer(_property.Data);
            if (_targetTransform == null)
                _property.StateMachine.ChangeState(E_GhoulState.Partrol);
            else
                _owner.StartCoroutine(StartAttack());
        }

        private void GetBehaviour()
        {
            var animator = _property.Animator;
            var attackBehaviour = animator.GetBehaviour<GhoulAttackBehaviour>();
            if (attackBehaviour != null)
                attackBehaviour.GhoulAttack = this;
        }

        public Transform GetTargetTransform()
        {
            if (_targetTransform == null)
                _targetTransform = FindPlayer(_property.Data);
            return _targetTransform;
        }

        private IEnumerator StartAttack()
        {
            var animator = _property.Animator;
            animator.SetTrigger(_attack);

            yield return new WaitUntil(() =>
            {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsTag("Shoot_bow") && stateInfo.normalizedTime >= 0.9f;
            });

            _property.StateMachine.ChangeState(E_GhoulState.CoolDown);
        }

        public void Init()
        {
            GetBehaviour();
        }
    }
}


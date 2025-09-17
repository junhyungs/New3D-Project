using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class SlamSlow : ForestMother_Pattern, IStateBehaviourController
    {
        private bool _isRotation;

        public override void Init(ForestMother owner)
        {
            base.Init(owner);
            GetBehaviour(owner.Property);
        }

        public override void Start()
        {
            IsRunning = true;
            _owner.StartCoroutine(WaitForAnimation());
        }

        public override IEnumerator WaitForAnimation()
        {
            PlayAnimation(MotherParameterKey.StartSlamSlow_Bool_true);
            yield return new WaitUntil(() => _isRotation);

            PlayAnimation(MotherParameterKey.StartSlamSlow_Bool_false);
            yield return new WaitUntil(() =>
            {
                var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(1);
                return stateInfo.IsName("Slam_slow_end") && stateInfo.normalizedTime >= 0.58f;
            });

            yield return _delay;
            IsRunning = false;
        }

        public override void Exit()
        {
            _isRotation = false;
        }

        public override void Enable()
        {
            GetBehaviour(_property);
        }

        private void RotationComplete() => _isRotation = true;
        private void GetBehaviour(ForestMotherProperty property)
        {
            var animator = property.Animator;
            if(animator != null)
            {
                var stateMachineBehaviour = animator.GetBehaviour<SlamslowBehaviour>();
                stateMachineBehaviour.Controller = this;
            }
        }

        public void OnEnter(Animator animator, AnimatorStateInfo stateInfo)
        {
            _property.AnimController.RotateForDuration(20f, 5f, RotationComplete);
        }
    }
}


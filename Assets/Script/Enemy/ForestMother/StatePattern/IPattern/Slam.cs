using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class Slam : ForestMother_Pattern
    {
        public override void Start()
        {
            IsRunning = true;
            _owner.StartCoroutine(WaitForAnimation());
        }

        public override IEnumerator WaitForAnimation()
        {
            PlayAnimation(MotherParameterKey.Slam_Trigger);
            yield return new WaitUntil(() =>
            {
                var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsTag("EndSpin") && stateInfo.normalizedTime >= 0.85f;
            });

            yield return _delay;
            IsRunning = false;
        }
    }
}


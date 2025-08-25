using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class BatDeath : BatState, ICharacterState<BatDeath>
    {
        public BatDeath(Bat owner) : base(owner) { }
        private readonly int _death = Animator.StringToHash("Death");

        public void OnStateEnter()
        {
            _owner.StopAllCoroutines();
            _property.NavMeshAgent.isStopped = true;

            if (IsMaterialValueBelow(5))
                _property.CopyMaterial.SetFloat(MATERIAL_PROPERTY, 0.5f);

            _property.Animator.SetTrigger(_death);
            _owner.StartCoroutine(DissolveEffect(_owner, 3f, -0.5f));
        }

        private IEnumerator DissolveEffect(Bat owner, float duration, float targetValue)
        {
            owner.StartCoroutine(owner.DissolveEffect(_property.CopyMaterial, duration, targetValue, MATERIAL_PROPERTY));
            yield return WaitForDissolve();
        }

        private IEnumerator WaitForDissolve()
        {
            yield return new WaitUntil(() => IsMaterialValueBelow(-5));
            //TODO EnemyPool·Î ¹ÝÈ¯.
        }

        private bool IsMaterialValueBelow(int compareValue)
        {
            var getfloat = _property.CopyMaterial.GetFloat(MATERIAL_PROPERTY) * 10;
            return getfloat <= compareValue;
        }
    }
}


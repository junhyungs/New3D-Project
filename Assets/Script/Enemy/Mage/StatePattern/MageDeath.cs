using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class MageDeath : MageState, ICharacterState<MageDeath>
    {
        public MageDeath(Mage mage) : base(mage) { }

        private readonly int _death = Animator.StringToHash("Death");

        public void OnStateEnter()
        {
            _property.Owner.StopAllCoroutines();
            _property.NavMeshAgent.isStopped = true;

            if (IsMaterialValueBelow(5))
                _property.CopyMaterial.SetFloat(PROPERTYNAME, 0.5f);

            _property.Animator.SetTrigger(_death);
            _property.Owner.StartCoroutine(DissolveEffect(_property.Owner, 3f, -0.5f));
        }

        private IEnumerator DissolveEffect(Mage owner, float duration, float targetValue)
        {
            owner.StartCoroutine(owner.DissolveEffect(_property.CopyMaterial, duration, targetValue, PROPERTYNAME));
            yield return WaitForDissolve();
        }
            
        private IEnumerator WaitForDissolve()
        {
            yield return new WaitUntil(() => IsMaterialValueBelow(-5));
            //TODO EnemyPool·Î ¹ÝÈ¯.
        }

        private bool IsMaterialValueBelow(int compareValue)
        {
            var getfloat = _property.CopyMaterial.GetFloat(PROPERTYNAME) * 10;
            return getfloat <= compareValue;
        }
            
    }
}


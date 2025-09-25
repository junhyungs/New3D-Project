using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyComponent
{
    public class ForestMotherDeath : ForestMotherState, ICharacterState<ForestMotherDeath>
    {
        public ForestMotherDeath(ForestMother owner) : base(owner) { }        

        public void OnStateEnter()
        {
            Death();
        }

        protected override void Death()
        {
            _property.AnimController.SetUpperWeight(0f);

            _owner.StopAllCoroutines();
            _property.NavMeshAgent.isStopped = true;

            if(NavMesh.SamplePosition(
                _property.StartPosition,
                out NavMeshHit hit,
                1f,
                NavMesh.AllAreas))
            {
                _property.NavMeshAgent.Warp(hit.position);
                _owner.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
                _property.NavMeshAgent.Warp(_property.StartPosition);

            if (IsMaterialValueBelow(5))
                _property.CopyMaterial.SetFloat(MATERIAL_PROPERTY, 0.5f);

            var animKey = MotherParameterKey.Death_Trigger.ToString();
            _property.AnimController.PlayAnimation(animKey);
            _owner.StartCoroutine(DissolveEffect(5f, -0.5f));
        }

        protected override void DisableObject()
        {
            _owner.gameObject.SetActive(false);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumCollection;

namespace EnemyComponent
{
    public class MageTeleport : MageState, ICharacterState<MageTeleport>
    {
        public MageTeleport(Mage mage) : base(mage) { }

        private readonly int _teleportIn = Animator.StringToHash("Teleport");
        private readonly int _teleportOut = Animator.StringToHash("TeleportOut");
        
        public void OnStateEnter()
        {
            _targetTransform = FindPlayer(_property.Data);
            if (_targetTransform == null)
                _property.StateMachine.ChangeState(E_MageState.Idle);

            _owner.StartCoroutine(Teleportation(_owner));
        }

        private IEnumerator Teleportation(Mage owner)
        {
            yield return owner.StartCoroutine(Teleportation_In(owner));
            yield return owner.StartCoroutine(Teleportation_Out(owner));
        }

        private IEnumerator Teleportation_In(Mage owner)
        {
            SetLayer(owner, "Teleport");
            TriggerAnimation(_teleportIn);

            yield return DissolveEffect(owner, 2.5f, -0.5f);
        }

        private IEnumerator Teleportation_Out(Mage owner)
        {
            var randomOffset = Random.insideUnitSphere * _property.Data.TeleportDistance;
            randomOffset.y = 0f;

            var teleportPosition = _targetTransform.position + randomOffset;
            if(NavMesh.SamplePosition(teleportPosition, out NavMeshHit hit, _property.Data.TeleportDistance, NavMesh.AllAreas))
            {
                _property.NavMeshAgent.Warp(hit.position);

                var lookDirection = (_targetTransform.position - owner.transform.position).normalized;
                lookDirection.y = 0f;
                if(lookDirection != Vector3.zero)
                    owner.transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            owner.StartCoroutine(DissolveEffect(owner, 1f, 0.5f));
            yield return WaitForDissolve(0f);

            TriggerAnimation(_teleportOut);
            yield return WaitForDissolve(0.5f);

            SetLayer(owner, "Enemy");
            _property.StateMachine.ChangeState(E_MageState.Move);
        }

        private void SetLayer(Mage owner, string LayerName) => 
            owner.gameObject.layer = LayerMask.NameToLayer(LayerName);
        private void TriggerAnimation(int hash) => 
            _property.Animator.SetTrigger(hash);

        private IEnumerator DissolveEffect(Mage owner, float duration, float targetValue) =>
            owner.DissolveEffect(_property.CopyMaterial, duration, targetValue, MATERIAL_PROPERTY);

        private IEnumerator WaitForDissolve(float threshold)
        {
            yield return new WaitUntil(() => _property.CopyMaterial.GetFloat(MATERIAL_PROPERTY) >= threshold);
        }
    }
}


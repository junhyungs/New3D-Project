using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public interface IBatAttackStateEventReceiver
    {
        void OnAttackAnimEnter();
        void OnAttackAnimUpdate(AnimatorStateInfo stateInfo);
        void OnAttackAnimExit();
    }

    public class BatAttack : BatState, ICharacterState<BatAttack>,
        IInitializeEnable, IBatAttackStateEventReceiver, IUnbindAction
    {
        public BatAttack(Bat owner) : base(owner)
        {
            GetBehaviour();

            var animEvent = owner.GetComponent<BatAnimationEvent>();
            if (animEvent != null)
                animEvent.AttackEvent += Attack;
        }

        public event Func<IEnumerator> StartCoolDown;
        private Vector3 _startPosition;
        private Vector3 _destination;

        private readonly int _bite = Animator.StringToHash("Bite");

        public void OnStateEnter()
        {
            _property.Animator.SetTrigger(_bite);
        }

        public void OnStateUpdate()
        {
            var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsTag("Bite") && stateInfo.normalizedTime >= 0.8f)
            {
                _owner.StartCoroutine(StartCoolDown.Invoke());
                _property.StateMachine.ChangeState(E_BatState.Trace);
            }
        }

        private void GetBehaviour()
        {
            var animator = _property.Animator;
            var biteBehaviour = animator.GetBehaviour<BiteBehaviour>();
            if (biteBehaviour != null)
                biteBehaviour.Receiver = this;
        }

        public void Init()
        {
            GetBehaviour();
        }

        public void OnAttackAnimEnter()
        {
            _destination = _property.TargetTransform.position;
            _startPosition = _owner.transform.position;

            AgentSetting(0f, 10f, 20f);
        }

        public void OnAttackAnimUpdate(AnimatorStateInfo stateInfo)
        {
            bool isStart = stateInfo.normalizedTime > 0.3f && stateInfo.normalizedTime <= 0.5f;
            if (!isStart)
                return;

            _property.NavMeshAgent.SetDestination(_destination);
        }

        public void OnAttackAnimExit()
        {
            var data = _property.Data;
            AgentSetting(data.AgentStopDistance, data.Speed);
        }

        private void Attack()
        {
            var center = (_startPosition + _destination) / 2 + Vector3.up;

            var width = _property.NavMeshAgent.radius * 2f;
            var height = width;
            var depth = Vector3.Distance(_startPosition, _destination);
            var boxSize = new Vector3(width, height, depth);

            var results = new Collider[1];
            var playerLayer = LayerMask.GetMask("Player");
            var hitCount = Physics.OverlapBoxNonAlloc(
                center, 
                boxSize / 2f,
                results,
                _owner.transform.rotation,
                playerLayer);
            if(hitCount > 0 && results[0].TryGetComponent(out ITakeDamage target))
            {
                target.TakeDamage(_property.Data.Damage);
            }
        }

        public void Unbind()
        {
            var animEvent = _owner.GetComponent<BatAnimationEvent>();
            if (animEvent != null)
                animEvent.AttackEvent -= Attack;
        }
    }
}


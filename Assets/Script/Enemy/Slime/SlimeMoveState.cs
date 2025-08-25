using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class SlimeMoveState : Slime_CheckPlayer, IStateBehaviourController,
        IInitializeEnable
    {
        public SlimeMoveState(Slime owner) : base(owner)
        {
            GetBehaviour();
        }

        protected SlimeHopStateBehaviour _stateBehaviour;
        private readonly int _hop = Animator.StringToHash("Hop");

        protected void PlayAnimation(bool isPlay) =>
            _property.Animator.SetBool(_hop, isPlay);

        protected void SetDestination(Vector3 destination) =>
            _property.NavMeshAgent.SetDestination(destination);

        protected bool HasReachedDestination(Vector3 destination)
        {
            return Vector3.Distance(destination, _owner.transform.position)
                <= _property.NavMeshAgent.stoppingDistance;
        }

        protected void GetBehaviour()
        {
            var animator = _property.Animator;
            _stateBehaviour = animator.GetBehaviour<SlimeHopStateBehaviour>();
        }

        protected void SetBehaviour()
        {
            if(_stateBehaviour != null)
                _stateBehaviour.Controller = this;
        }

        protected void AnimationMovement(AnimatorStateInfo info, float stoppingDistance,
            float speed, float acceleration)
        {
            float time = info.normalizedTime % 1f;
            bool canMove = time >= 0.3f && time <= 0.6f;
            if (canMove)
                AgentSetting(stoppingDistance, speed, acceleration);
            else
                AgentSetting(stoppingDistance, 0f, acceleration);
        }

        public virtual void OnEnter(Animator animator, AnimatorStateInfo stateInfo) { }
        public virtual void OnUpdate(Animator animator, AnimatorStateInfo stateInfo) { }
        public virtual void OnExit(Animator animator, AnimatorStateInfo stateInfo)
        {
            SetDestination(_owner.transform.position);
        }

        public void Init()
        {
            GetBehaviour();
        }
    }
}


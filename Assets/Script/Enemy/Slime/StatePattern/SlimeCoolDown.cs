using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class SlimeCoolDown : SlimeState, ICharacterState<SlimeCoolDown>
    {
        public SlimeCoolDown(Slime owner) : base(owner) { }
        
        private float _changeTime;
        private const float TIME = 1.5f;

        public void OnStateEnter()
        {
            _changeTime = Time.time + TIME;
        }

        public void OnStateUpdate()
        {
            var target = _property.TargetTransform;

            bool canChange = Time.time > _changeTime;
            if (canChange)
            {
                var distance = Vector3.Distance(target.position, _owner.transform.position);
                if (distance <= _property.NavMeshAgent.stoppingDistance)
                    _property.StateMachine.ChangeState(E_SlimeState.Attack);
                else
                    _property.StateMachine.ChangeState(E_SlimeState.Trace);
            }
            else
            {
                Vector3 rotateDirection = (target.position - _owner.transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(rotateDirection);
                _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                    rotation, 5f * Time.deltaTime);
            }
        }
    }
}


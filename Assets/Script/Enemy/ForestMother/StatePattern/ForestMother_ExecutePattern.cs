using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class ForestMother_ExecutePattern : ForestMotherState, ICharacterState<ForestMother_ExecutePattern>
    {
        public ForestMother_ExecutePattern(ForestMother owner) : base(owner) { }
        
        public void OnStateEnter()
        {
            _property.CurrentPattern.Start();
        }

        public void OnStateUpdate()
        {
            if (!_property.CurrentPattern.IsRunning)
                _property.StateMachine.ChangeState(E_ForestMotherState.ChangePattern);
            else
                _property.CurrentPattern.Update();
        }

        public void OnStateExit()
        {
            _property.CurrentPattern.Exit();
        }

        public void OnTriggerEnter(Collider other)
        {
            _property.CurrentPattern.OnTriggerEnter(other);
        }
    }
}


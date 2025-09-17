using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class ForestMother_ChangePattern : ForestMotherState, ICharacterState<ForestMother_ChangePattern>,
        IInitializeEnable
    {
        public ForestMother_ChangePattern(ForestMother owner) : base(owner)
        {
            _manager = new ForestMotherPatternManager(owner);
        }

        private ForestMotherPatternManager _manager;

        public void OnStateEnter()
        {
            _property.CurrentPattern = _manager.GetPattern;
            _property.StateMachine.ChangeState(E_ForestMotherState.ExecutePattern);
        }

        public void Init()
        {
            _manager.Enable();
        }
    }
}


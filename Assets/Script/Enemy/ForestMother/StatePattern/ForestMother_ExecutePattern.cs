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
            _property.Pattern.Start();
        }

        public void OnStateUpdate()
        {
            _property.Pattern.Update();
        }

        public void OnStateExit()
        {
            _property.Pattern.Exit();
        }
    }
}


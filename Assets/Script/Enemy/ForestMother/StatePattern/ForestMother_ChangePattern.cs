using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class ForestMother_ChangePattern : ForestMotherState, ICharacterState<ForestMother_ChangePattern>
    {
        public ForestMother_ChangePattern(ForestMother owner) : base(owner) { }
    }
}


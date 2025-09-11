using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class ForestMother_GeneratePattern : ForestMotherState, ICharacterState<ForestMother_GeneratePattern>
    {
        public ForestMother_GeneratePattern(ForestMother owner) : base(owner) { }
    }
}

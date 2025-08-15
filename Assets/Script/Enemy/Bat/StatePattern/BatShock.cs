using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class BatShock : BatState, ICharacterState<BatShock>
    {
        public BatShock(Bat owner) : base(owner) { }
    }
}


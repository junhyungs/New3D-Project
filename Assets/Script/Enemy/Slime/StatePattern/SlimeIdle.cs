using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class SlimeIdle : Slime_CheckPlayer, ICharacterState<SlimeIdle>
    {
        public SlimeIdle(Slime owner) : base(owner) { }
    }
}


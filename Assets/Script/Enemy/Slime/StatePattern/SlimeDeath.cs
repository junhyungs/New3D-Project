using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class SlimeDeath : SlimeState, ICharacterState<SlimeDeath>
    {
        public SlimeDeath(Slime owner) : base(owner)
        {
        }

     
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class BatDeath : BatState, ICharacterState<BatDeath>
    {
        public BatDeath(Bat owner) : base(owner) { }
    }
}


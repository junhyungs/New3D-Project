using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class ForestMotherDeath : ForestMotherState, ICharacterState<ForestMotherDeath>
    {
        public ForestMotherDeath(ForestMother owner) : base(owner) { }
        private readonly int _death = Animator.StringToHash("Death");

        public void OnStateEnter()
        {
            Death(_death);
        }
    }
}


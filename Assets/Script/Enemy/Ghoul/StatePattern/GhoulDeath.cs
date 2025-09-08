using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
namespace EnemyComponent
{
    public class GhoulDeath : GhoulState, ICharacterState<GhoulDeath>
    {
        public GhoulDeath(Ghoul owner) : base(owner) { }
        private readonly int _death = Animator.StringToHash("Death");

        public void OnStateEnter()
        {
            Death(_death);
        }
    }
}

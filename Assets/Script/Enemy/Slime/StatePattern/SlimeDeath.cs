using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class SlimeDeath : SlimeState, ICharacterState<SlimeDeath>
    {
        public SlimeDeath(Slime owner) : base(owner) { }
        private readonly int _death = Animator.StringToHash("Death");
        
        public void OnStateEnter()
        {
            Death(_death);
        }
    }
}


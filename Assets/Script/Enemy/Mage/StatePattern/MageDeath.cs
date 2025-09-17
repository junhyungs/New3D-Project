using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class MageDeath : MageState, ICharacterState<MageDeath>
    {
        public MageDeath(Mage mage) : base(mage) { }

        private readonly int _death = Animator.StringToHash("Death");

        public void OnStateEnter()
        {
           Death(_death);
        }
    }
}


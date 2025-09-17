using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class MageState : EnemyState<MageProperty, Mage, MageStateMachine, E_MageState>
    {
        public MageState(Mage mage) : base(mage) { }
        
        protected Transform _targetTransform;
    }
}

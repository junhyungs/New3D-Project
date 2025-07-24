using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class Mage : Enemy<MageProperty>
    {
        [Header("TestData")] //�׽�Ʈ �ڵ�
        public MageSO _testData;
        protected override MageProperty CreateProperty()
        {
            return new MageProperty(this);
        }

        protected override void Death()
        {
            Property.StateMachine.ChangeState(E_MageState.Death);
        }
    }
}


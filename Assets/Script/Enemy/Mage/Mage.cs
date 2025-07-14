using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class Mage : Enemy<MageStateMachine, E_MageState>
    {
        [Header("TestData")] //�׽�Ʈ �ڵ�
        public EnemyDataSO _testData;
        public bool IsSpawn { get; set; }
    }
}


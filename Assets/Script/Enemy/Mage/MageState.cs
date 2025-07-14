using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class MageState
    {
        public MageState(Mage mage)
        {
            _mage = mage;
            _mageTransform = mage.transform;
            _stateMachine = _mage.StateMachine;

            SetData();
        }

        protected Mage _mage;
        protected MageStateMachine _stateMachine;
        protected EnemyDataSO _data;
        protected Transform _mageTransform;

        private void SetData()
        {
            //var key = ScriptableDataKey.MageSO;
            //var enemyDataSO = DataManager.Instance.GetScriptableData(key) as EnemyDataSO;
            //if(enemyDataSO != null)
            //    _data = enemyDataSO;

            _data = _mage._testData; //테스트 코드
        }

        protected float GetRange()
        {
            var range = _mage.IsSpawn ? _data.Spawn_DetectionRange :
                _data.DetectionRange;
            return range;
        }

        protected Transform FindTarget()
        {
            var targetLayer = LayerMask.GetMask("Player");
            var range = GetRange(); 
            var results = new Collider[1];

            var count = Physics.OverlapSphereNonAlloc(_mageTransform.position,
                range, results, targetLayer);
            if(count > 0)
            {
                var targetTransform = results[0].transform;
                return targetTransform;
            }
            else
                return null;
        }
    }
}

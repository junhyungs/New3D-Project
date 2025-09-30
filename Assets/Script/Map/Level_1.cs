using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace MapComponent
{
    public class Level_1 : MapBase<Level_1>
    {
        [Header("IntroBossObject")]
        [SerializeField] private GameObject _introBossObject;

        [Header("HitTrigger")]
        [SerializeField] private HitTrigger[] _triggers;
        [SerializeField] private SpikeDoor _bossDoor;

        private HashSet<HitTrigger> _hitTriggerSet = new HashSet<HitTrigger>();

        protected override void AdditionalInit(LevelData levelData)
        {
            levelData.MapEventDictionary[GameEvent.ForestMotherBoss] = false;
        }

        protected override void OnStartMap()
        {
            _myLevelData.Initialize = true;

            InitIntroBossObject();
            InitHitTrigger();
        }

        private void InitIntroBossObject()
        {
            var mapEventDic = _myLevelData.MapEventDictionary;
            var clearBoss = mapEventDic[GameEvent.ForestMotherBoss];
            _introBossObject.SetActive(!clearBoss);
        }

        private void InitHitTrigger()
        {
            if (_triggers == null)
                return;

            foreach (var trigger in _triggers)
            {
                if (trigger != null)
                {
                    trigger.HitAction += UnRegisterHitTriggerSet;
                    _hitTriggerSet.Add(trigger);
                }
            }
        }

        private void UnRegisterHitTriggerSet(HitTrigger hitTrigger)
        {
            if (!_hitTriggerSet.Contains(hitTrigger))
                return;

            hitTrigger.HitAction -= UnRegisterHitTriggerSet;
            _hitTriggerSet.Remove(hitTrigger);
            if (_hitTriggerSet.Count <= 0)
                BossDoor(true);
        }

        public void BossDoor(bool value)
        {
            //Action action = value ? _bossDoor.HitTrigger :
            //    _bossDoor.CloseDoor;
            
            //action.Invoke();
        }
    }
}


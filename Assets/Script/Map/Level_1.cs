using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace MapComponent
{
    public class Level_1 : MapBase<Level_1_progress>
    {
        [Header("IntroBossObject")]
        [SerializeField] private GameObject _introBossObject;

        [Header("HitTrigger")]
        [SerializeField] private HitTrigger[] _triggers;
        [SerializeField] private SpikeDoor _bossDoor;

        private HashSet<HitTrigger> _hitTriggerSet = new HashSet<HitTrigger>();

        protected override void OnStartMap()
        {
            InitIntroBossObject();
            InitHitTrigger();
        }

        protected override void AdditionalInit(Level_1_progress progress)
        {
            progress.ClearedObjects = new HashSet<string>();
        }

        private void InitIntroBossObject()
        {
            var isClear = _myProgress.ClearBoss;
            _introBossObject.SetActive(!isClear);
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


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
        [Header("HitTrigger")]
        [SerializeField] private HitTrigger[] _triggers;
        private HashSet<HitTrigger> _hitTriggerSet = new HashSet<HitTrigger>();
        [SerializeField] private SpikeDoor _bossDoor;

        protected override void OnStartMap()
        {
            InitHitTrigger();
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
            Action action = value ? _bossDoor.HitTrigger :
                _bossDoor.CloseDoor;
            
            action.Invoke();
        }
    }
}


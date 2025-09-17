using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace MapComponent
{
    public class Level_1 : MapBase<Level_1_progress>
    {
        [Header("HitTrigger")]
        [SerializeField] private HitTrigger[] _triggers;
        private HashSet<HitTrigger> _hitTriggerSet = new HashSet<HitTrigger>();
        [SerializeField] private SpikeDoor _bossDoor;

        public Level_1_progress Progress => _myProgress;

        public override void Initialize(Dictionary<string, MapProgress> progressDictionary)
        {
            if(!progressDictionary.TryGetValue(nameof(Level_1), out var progress))
            {
                progress = new Level_1_progress();
                progressDictionary.Add(nameof(Level_1), progress);
            }

            _myProgress = progress as Level_1_progress;
        }

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
                    trigger.HitAction += UnRegisterHitTriggerSet;
            }
        }

        private void UnRegisterHitTriggerSet(HitTrigger hitTrigger)
        {
            if (!_hitTriggerSet.Contains(hitTrigger))
                return;

            hitTrigger.HitAction -= UnRegisterHitTriggerSet;
            _hitTriggerSet.Remove(hitTrigger);
            if (_hitTriggerSet.Count <= 0)
                _bossDoor.HitTrigger();
        }

        protected override void PlayDoorTimeLine()
        {
            if (LinkedDoor == LinkedDoor.Default)
                return;

            OutTimeLine();
        }
    }
}


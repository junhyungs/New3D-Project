using GameData;
using MapComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EventClass
{
    [Serializable]
    public class DoorEventObjects
    {
        [Header("HitObjects")]
        public HitTrigger[] HitObjects;
        [Header("LastTargetObject")]
        public GameObject LastTargetObject;
        [HideInInspector]
        public LevelData LevelData;
    }

    public class DoorEvent
    {
        public DoorEvent(DoorEventObjects eventObjects)
        {
            var levelData = eventObjects.LevelData;
            if (levelData == null)
                return;

            var clearedObjectSet = levelData.ClearedObjects;
            bool isAllClear = true;
            foreach(var hitObject in eventObjects.HitObjects)
            {
                var interactionList = hitObject.HitInteractions;
                bool isClear = true;
                foreach(var item in interactionList)
                {
                    var uniqueId = item.UniqueObjectID.Id;
                    if (clearedObjectSet.Contains(uniqueId))
                        item.GameObject.SetActive(false);
                    else
                        isClear = false;
                }

                if (!isClear)
                {
                    isAllClear = false;
                    hitObject.HitAction += RemoveHitSet;
                    _hitSet.Add(hitObject);
                }
            }

            var lastTargetObject = eventObjects.LastTargetObject;
            if (lastTargetObject.TryGetComponent(out IHitInteraction interaction))
            {
                Action afterAction = null;
                afterAction = () =>
                {
                    interaction.OnHit();
                    AfterEvent -= afterAction;
                };
                AfterEvent += afterAction;
            }

            if (isAllClear)
                AfterEvent?.Invoke();
            else
                _addAction = (value) => clearedObjectSet.Add(value);
        }

        private HashSet<HitTrigger> _hitSet = new HashSet<HitTrigger>();
        private Action<string> _addAction;
        public event Action AfterEvent;

        private void Complete(HitTrigger hitTrigger)
        {
            var hitInteractions = hitTrigger.HitInteractions;
            foreach(var item in hitInteractions)
            {
                var uniqueId = item.UniqueObjectID.Id;
                _addAction?.Invoke(uniqueId);
            }
        }

        private void RemoveHitSet(HitTrigger hitTrigger)
        {
            if (!_hitSet.Contains(hitTrigger))
                return;

            Complete(hitTrigger);

            hitTrigger.HitAction -= RemoveHitSet;
            _hitSet.Remove(hitTrigger);
            if (_hitSet.Count <= 0)
            {
                AfterEvent?.Invoke();
                ExitDoorEvent();
            }
        }

        public void ExitDoorEvent()
        {
            _addAction = null;
            AfterEvent = null;
            _hitSet.Clear();
        }
    }
}


using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;
using EventClass;

namespace MapComponent
{
    public class Level_1 : MapBase<Level_1>
    {
        [Header("IntroBossObject")]
        [SerializeField] private GameObject _introBossObject;

        [Header("HitObjects")]
        [SerializeField] private DoorEventObjects _doorEventObjects;
        private DoorEvent _doorEvent;

        protected override void OnStartMap()
        {
            _myLevelData.Initialize = true;

            InitIntroBossObject();
            InitHitObjects();
        }

        protected override void OnDestroyMap()
        {
            _doorEvent.ExitDoorEvent();
        }

        private void InitIntroBossObject()
        {
            var mapEventDic = _myLevelData.MapEventDictionary;
            if (mapEventDic.TryGetValue(GameEvent.ForestMotherBoss, out bool value) &&
                value)
                _introBossObject.SetActive(false);
        }

        private void InitHitObjects()
        {
            _doorEventObjects.LevelData = _myLevelData;
            _doorEvent = new DoorEvent(_doorEventObjects);
        }

        public void BossDoor(bool value)
        {
            if (_doorEventObjects == null)
                return;

            var bossDoorObject = _doorEventObjects.LastTargetObject;
            if(bossDoorObject.TryGetComponent(out IHitInteraction_Door component))
            {
                Action action = value ? component.OnHit :
                    component.CloseDoor;
                action?.Invoke();
            }
        }
    }
}


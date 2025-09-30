using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace MapComponent
{
    public interface IMap
    {
        bool PlayTimeLine { get; set; }
        LinkedDoor LinkedDoor { get; set; }
        void Init(Dictionary<string, LevelData> levelDictionary);
        void ActiveDoor();
    }

    public abstract class MapBase<TMapType> : MonoBehaviour , IMap
        where TMapType : IMap, new()
    {
        [Header("ShortcutDoor"), SerializeField]
        protected ShortCutDoorInfo[] _shortcutDoors;
        protected Dictionary<LinkedDoor, ShortCutDoor> _doorDictionary;
        protected LevelData _myLevelData;

        public bool PlayTimeLine { get; set; }
        public LinkedDoor LinkedDoor { get; set; }
        public LevelData MapLevelData  => _myLevelData;

        public void Init(Dictionary<string, LevelData> levelDictionary)
        {
            string typeName = typeof(TMapType).Name;
            if (!levelDictionary.TryGetValue(typeName, out var levelData))
            {
                levelData = new LevelData()
                {
                    OpenDoor = new List<LinkedDoor>(),
                    MapEventDictionary = new Dictionary<GameEvent, bool>(),
                    ClearedObjects = new HashSet<string>()
                };
                AdditionalInit(levelData);
                levelDictionary.Add(typeName, levelData);
            }

            _myLevelData = levelData;
            InitDoor();
        }

        public void ActiveDoor()
        {
            foreach(var item in _myLevelData.OpenDoor)
            {
                var door = GetDoor(item);
                if (door != null)
                    door.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            OnStartMap();
        }

        private void OnEnable()
        {
            DoorTimeLine();
        }

        protected virtual void AdditionalInit(LevelData levelData) { }
        protected virtual void OnStartMap() { }
        protected virtual void DoorTimeLine()
        {
            if (!PlayTimeLine || LinkedDoor == LinkedDoor.Default)
                return;

            OutTimeLine(LinkedDoor);
        }

        protected void OutTimeLine(LinkedDoor linkedDoor)
        {
            var shortcutDoor = GetDoor(linkedDoor);
            shortcutDoor.PlayOutTimeLine();
        }

        private void InitDoor()
        {
            _doorDictionary = new Dictionary<LinkedDoor, ShortCutDoor>();
            foreach (var door in _shortcutDoors)
                if (door != null)
                    _doorDictionary.Add(door.LinkedDoor, door.ShortCutDoor);
                else
                {
                    LoadSceneManager.Instance.LoadSceneAndReportError("StartScene", "DoorError");
                    break;
                }
        }

        public ShortCutDoor GetDoor(LinkedDoor door)
        {
            return _doorDictionary[door];
        }
    }

    [System.Serializable]
    public class ShortCutDoorInfo
    {
        public LinkedDoor LinkedDoor;
        public ShortCutDoor ShortCutDoor;
    }
}

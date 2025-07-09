using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace MapComponent
{
    public class Map : MonoBehaviour
    {
        public LinkedDoor LinkedDoor { get; set; }
        public virtual void Initialize(Dictionary<string, MapProgress> progressDictionary) { }
    }

    public class MapBase<T> : Map where T : MapProgress
    {
        [Header("ShortcutDoor"), SerializeField]
        protected ShortCutDoorInfo[] _shortcutDoors;
        protected Dictionary<LinkedDoor, ShortCutDoor> _doorDictionary;

        protected T _myProgress;

        private void Awake()
        {
            InitDoor();
        }

        protected virtual void OnEnable()
        {
            PlayDoorTimeLine();
        }

        protected virtual void PlayDoorTimeLine()
        {
            bool init = _myProgress.Initialize;
            if (!init || LinkedDoor == LinkedDoor.Default)
                return;

            OutTimeLine();
        }

        protected void OutTimeLine()
        {
            var shortcutDoor = GetDoor(LinkedDoor);
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

        protected ShortCutDoor GetDoor(LinkedDoor door)
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

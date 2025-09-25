using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace MapComponent
{
    public interface IMap
    {
        bool PlayTimeLine { get; set; }
        LinkedDoor LinkedDoor { get; set; }
        void Init(Dictionary<string, MapProgress> progressDic);
    }

    public abstract class MapBase<TProgress> : MonoBehaviour , IMap
        where TProgress : MapProgress, new()
    {
        [Header("ShortcutDoor"), SerializeField]
        protected ShortCutDoorInfo[] _shortcutDoors;
        protected Dictionary<LinkedDoor, ShortCutDoor> _doorDictionary;
        protected TProgress _myProgress;

        public bool PlayTimeLine { get; set; }
        public LinkedDoor LinkedDoor { get; set; }
        public TProgress MapProgress  => _myProgress;

        public void Init(Dictionary<string, MapProgress> progressDic)
        {
            if (!progressDic.TryGetValue(typeof(TProgress).Name, out var progress))
            {
                progress = new TProgress();
                progress.OpenDoor = new List<LinkedDoor>();
                progressDic.Add(typeof(TProgress).Name, progress);
            }

            _myProgress = progress as TProgress;
            ActiveDoor(_myProgress.OpenDoor);
        }

        protected void ActiveDoor(List<LinkedDoor> list)
        {
            foreach(var item in list)
            {
                var door = GetDoor(item);
                if (door != null)
                    door.gameObject.SetActive(true);
            }
        }

        private void Awake()
        {
            InitDoor();
            OnAwakeMap();
        }

        private void Start()
        {
            OnStartMap();
        }

        private void OnEnable()
        {
            OnEnableMap();
            DoorTimeLine();
        }

        protected virtual void OnAwakeMap() { }
        protected virtual void OnEnableMap() { }
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

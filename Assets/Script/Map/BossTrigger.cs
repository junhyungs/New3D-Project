using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeLineComponent;
using EnumCollection;

namespace MapComponent
{
    public class BossTrigger : MapTrigger<BoxCollider>
    {       
        [Header("Level_1")]
        [SerializeField] private Level_1 _mapComponent;
        [Header("BossTimeLine")]
        [SerializeField] private TimeLine _motherTimeLine;

        private void Start()
        {
            if (_mapComponent == null)
                return;

            var levelData = _mapComponent.MapLevelData;
            if (levelData.MapEventDictionary.TryGetValue(GameEvent.ForestMotherBoss, out bool value))
                _collider.enabled = !value;
        }

        protected override void Trigger(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")
                || _motherTimeLine == null)
                return;

            _collider.enabled = false;
            
            var nextLevelDoor = _mapComponent.GetDoor(LinkedDoor.Level_1_Level_2);
            nextLevelDoor.gameObject.SetActive(false);
            _motherTimeLine.PlayTimeLine();
        }
    }
}


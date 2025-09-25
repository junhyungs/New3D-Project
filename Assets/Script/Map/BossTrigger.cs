using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeLineComponent;

namespace MapComponent
{
    public class BossTrigger : MapTrigger<BoxCollider>
    {       
        [Header("Level_1")]
        [SerializeField] private Level_1 _mapComponent;
        [Header("BossObject")]
        [SerializeField] private GameObject _boss;
        [Header("BossTimeLine")]
        [SerializeField] private TimeLine _motherTimeLine;
        [Header("Door")]
        [SerializeField] private GameObject _door;

        private void OnEnable()
        {
            _collider.enabled = true;
        }

        protected override void Trigger(Collider other)
        {
            if (_mapComponent == null ||
                _boss == null)
                return;

            _collider.enabled = false;
            var progress = _mapComponent.MapProgress;
            if (!progress.ClearBoss)
            {
                _motherTimeLine.PlayTimeLine();
            }
            else
                _boss.SetActive(true);

            _door.SetActive(false);
        }
    }
}


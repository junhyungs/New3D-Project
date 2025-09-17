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
        [SerializeField] private TimeLine _enableBoss;

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
            var progress = _mapComponent.Progress;
            if (!progress.ClearBoss)
            {
                _enableBoss.PlayTimeLine();
            }
            else
                _boss.SetActive(true);
        }
    }
}


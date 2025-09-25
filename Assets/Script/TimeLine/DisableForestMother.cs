using MapComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeLineComponent
{
    public class DisableForestMother : TimeLine
    {
        [Header("MapComponent")]
        [SerializeField] private Level_1 _mapComponent;
        public override void PlayTimeLine()
        {
            _mapComponent.BossDoor(true);
            _playableDirector.Play();
        }
    }
}


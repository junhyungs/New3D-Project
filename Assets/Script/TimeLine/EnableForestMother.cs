using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapComponent;

namespace TimeLineComponent
{
    public class EnableForestMother : TimeLine
    {
        [Header("MapComponent")]
        [SerializeField] private Level_1 _mapComponent;
        public override void PlayTimeLine()
        {
            _mapComponent.BossDoor(false);
            _playableDirector.Play();
        }
    }
}


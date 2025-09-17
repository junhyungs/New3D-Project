using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeLineComponent
{
    public class EnableForestMother : TimeLine
    {
        public override void PlayTimeLine()
        {
            _playableDirector.Play();
        }
    }
}


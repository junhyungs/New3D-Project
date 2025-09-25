using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace TimeLineComponent
{
    public class Mother_TimeLine : TimeLine
    {
        [Header("PlayableAsset")]
        [SerializeField] private PlayableAsset[] _assets;
        public override void PlayTimeLine()
        {
            _playableDirector.Play();
        }
    }
}


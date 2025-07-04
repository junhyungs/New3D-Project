using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace TimeLineComponent
{
    public abstract class TimeLine : MonoBehaviour
    {
        protected PlayableDirector _playableDirector;
        public abstract void PlayTimeLine();
        protected virtual void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }
    }
}


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
        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }
    }
}


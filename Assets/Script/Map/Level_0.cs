using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using UnityEngine.Playables;
using TimeLineComponent;

namespace MapComponent
{
    public class Level_0 : Map
    {
        [Header("TimeLine")]
        [SerializeField] private TimeLine _intro;
        [SerializeField] private TimeLine _hallCrow;

        private Level_0_progress _myProgress;
        public override void Initialize()
        {
            if (ProgressDictionary == null)
                return;

            if(!ProgressDictionary.TryGetValue(nameof(Level_0), out var progress))
                progress = new Level_0_progress();
            _myProgress = progress as Level_0_progress;
        }

        private void Start()
        {
            bool isStart = _myProgress.Initialize;
            if (!isStart)
            {
                //TODO
                _intro.PlayTimeLine();
                _myProgress.Initialize = true;
            }
        }
    }
}


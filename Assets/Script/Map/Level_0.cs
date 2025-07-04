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
        public override void Initialize(Dictionary<string, MapProgress> progressDictionary)
        {
            if(!progressDictionary.TryGetValue(nameof(Level_0), out var progress))
            {
                progress = new Level_0_progress();
                progressDictionary.Add(nameof(Level_0), progress);
            }

            _myProgress = progress as Level_0_progress;
        }

        private void Start()
        {
            bool isStart = _myProgress.Initialize;
            if (!isStart)
            {
                _intro.PlayTimeLine();
                _hallCrow.gameObject.SetActive(true);
                _myProgress.Initialize = true;
            }
        }
    }
}


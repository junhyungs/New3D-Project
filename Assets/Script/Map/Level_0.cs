using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using UnityEngine.Playables;
using TimeLineComponent;
using EnumCollection;

namespace MapComponent
{
    public class Level_0 : MapBase<Level_0_progress>
    {
        [Header("TimeLine")]
        [SerializeField] private TimeLine _intro;
        [SerializeField] private TimeLine _hallCrow;

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


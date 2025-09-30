using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using UnityEngine.Playables;
using TimeLineComponent;
using EnumCollection;

namespace MapComponent
{
    public class Level_0 : MapBase<Level_0>
    {
        [Header("TimeLine")]
        [SerializeField] private TimeLine _intro;
        [SerializeField] private TimeLine _hallCrow;

        private void Start()
        {
            if (!_myLevelData.Initialize)
            {
                _intro.PlayTimeLine();
                _hallCrow.gameObject.SetActive(true);
                _myLevelData.Initialize = true;
            }
        }
    }
}


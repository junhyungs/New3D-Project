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

        protected override void OnStartMap()
        {
            if (!_myLevelData.Initialize)
            {
                _myLevelData.Initialize = true;
                _intro.PlayTimeLine();
            }

            if (_myLevelData.MapEventDictionary.TryGetValue(GameEvent.HallCrow, out bool value)
                && value)
                _hallCrow.gameObject.SetActive(false);
        }
    }
}


using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace MapComponent
{
    public class Level_1 : MapBase<Level_1_progress>
    {
        //[Header("TimeLine")]
        public override void Initialize(Dictionary<string, MapProgress> progressDictionary)
        {
            if(!progressDictionary.TryGetValue(nameof(Level_1), out var progress))
            {
                progress = new Level_1_progress();
                progressDictionary.Add(nameof(Level_1), progress);
            }

            _myProgress = progress as Level_1_progress;
        }

        private void Start()
        {
            //TODO 타임라인 초기화
        }

        protected override void PlayDoorTimeLine()
        {
            if (LinkedDoor == LinkedDoor.Default)
                return;

            OutTimeLine();
        }
    }
}


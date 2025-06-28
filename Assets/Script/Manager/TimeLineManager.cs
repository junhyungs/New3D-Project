using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEngine.Playables;

public class TimeLineManager : Singleton<TimeLineManager>   
{
    private Dictionary<TimeLine, PlayableDirector> _timeLineDictionary = new Dictionary<TimeLine, PlayableDirector>();

    
}

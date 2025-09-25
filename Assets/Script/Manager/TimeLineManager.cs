using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEngine.Playables;

public class TimeLineManager : Singleton_MonoBehaviour<TimeLineManager>
{
    [System.Serializable]
    public class TimeLineInfo
    {
        [Header("PlayableAsset")]
        public PlayableAsset Asset;
        [Header("AssetName")]
        public PlayableAssetName Name;
    }

    [Header("TimeLineInfo")]
    [SerializeField] private TimeLineInfo[] _timeLineInfos;
    private Dictionary<PlayableAssetName, PlayableAsset> _playableAssetDictionary;

    private void Awake()
    {
        BindPlayableAsset();
    }

    private void BindPlayableAsset()
    {
        _playableAssetDictionary = new Dictionary<PlayableAssetName, PlayableAsset>();
        foreach(var info in _timeLineInfos)
            if(info != null)
                _playableAssetDictionary.Add(info.Name, info.Asset);
    }

    public PlayableAsset GetPlayableAsset(PlayableAssetName name)
    {
        if(_playableAssetDictionary.TryGetValue(name, out var asset))
            return asset;
        return null;
    }
}

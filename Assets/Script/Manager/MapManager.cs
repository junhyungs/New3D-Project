using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using GameData;
using MapComponent;

public class MapManager : MonoBehaviour
{
    private Dictionary<string, GameObject> _mapDictionary = new Dictionary<string, GameObject>();
    private GameObject _currentMap;

    private async void Start()
    {
        var key = DataKey.Map_Data.ToString();
        var mapData = DataManager.Instance.GetData(key) as MapData;

        var startMapName = mapData.CurrentMapObjectName != null ?
            mapData.CurrentMapObjectName : AddressablesKey.Map_Level_0;
        await LoadMapAsync(startMapName);
    }

    private void OnDestroy()
    {
        OnDestroyMapManager();
    }

    private void OnDestroyMapManager()
    {
        foreach (var mapInstance in _mapDictionary.Values)
            if(mapInstance != null)
                Addressables.ReleaseInstance(mapInstance);
    }

    public async UniTask LoadMapAsync(string addressableKey)
    {
        LoadSceneManager.Instance.StartLoadingUICoroutine(true);

        if(_currentMap != null)
            _currentMap.SetActive(false);
        
        if(!_mapDictionary.ContainsKey(addressableKey))
        {
            var handle = Addressables.InstantiateAsync(addressableKey);
            var mapIntance = await handle.ToUniTask();

            _mapDictionary.Add(addressableKey, mapIntance);
        }

        var nextMap = _mapDictionary[addressableKey];

        var dataKey = DataKey.Map_Data.ToString();
        var mapData = DataManager.Instance.GetData(dataKey) as MapData;
        mapData.CurrentMapObjectName = nextMap.name;

        if(nextMap.TryGetComponent(out Map mapCompnent))
        {
            mapCompnent.Initialize(mapData.ProgressDictionary);
        }
        else
        {
            LoadSceneManager.Instance.LoadSceneAndReportError("StartScene", "Map Error");
            return;
        }

        nextMap.SetActive(true);

        _currentMap = nextMap;
        LoadSceneManager.Instance.StartLoadingUICoroutine(false);
    }
}

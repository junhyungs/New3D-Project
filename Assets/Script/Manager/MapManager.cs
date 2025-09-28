using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using GameData;
using MapComponent;
using System;

public class MapManager : Singleton_MonoBehaviour<MapManager>
{
    private Dictionary<string, GameObject> _mapDictionary = new Dictionary<string, GameObject>();
    private GameObject _currentMap;

    private async void Start()
    {
        var key = DataKey.Map_Data.ToString();
        var mapData = DataManager.Instance.GetData(key) as MapData;
        if (mapData == null)
        {
            LoadSceneManager.Instance.LoadSceneAndReportError("StartScene", "MapData Error");
            return;
        }

        var startMapAddressables = mapData.MapAddressablesKey != null ?
            mapData.MapAddressablesKey : AddressablesKey.Map_Level_0;
        await LoadMapAsync(startMapAddressables, LinkedDoor.Default, false);
        PlayerManager.Instance.LoadPlayer(mapData);
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

    public void ChangeMapAsync(string addressableKey, LinkedDoor linkedDoor)
    {
        RunAsync(addressableKey, linkedDoor).Forget();
        async UniTask RunAsync(string addressableKey, LinkedDoor linkedDoor)
        {
            try
            {
                await LoadMapAsync(addressableKey, linkedDoor);
            }
            catch (Exception ex)
            {
                LoadSceneManager.Instance.LoadSceneAndReportError("StartScene", ex.Message);
            }
        }
    }

    public async UniTask LoadMapAsync(string addressableKey,
        LinkedDoor linkedDoor = LinkedDoor.Default, bool playDoorTimeLine = true)
    {
        LoadSceneManager.Instance.StartLoadingUICoroutine(true);

        if(_currentMap != null)
            _currentMap.SetActive(false);

        var dataKey = DataKey.Map_Data.ToString();
        var mapData = DataManager.Instance.GetData(dataKey) as MapData;

        IMap mapComponent = null;
        if (!_mapDictionary.ContainsKey(addressableKey))
        {        
            var handle = Addressables.InstantiateAsync(addressableKey);

            var mapObject = await handle.ToUniTask();
            if(mapObject.TryGetComponent(out mapComponent))
                mapComponent.Init(mapData.ProgressDictionary);
            else
            {
                LoadSceneManager.Instance.LoadSceneAndReportError("StartScene", "Map Error");
                return;
            }

            _mapDictionary.Add(addressableKey, mapObject);
        }
        else
        {
            var mapObject = _mapDictionary[addressableKey];
            mapComponent = mapObject.GetComponent<IMap>();
        }

        mapComponent.LinkedDoor = linkedDoor;
        mapComponent.PlayTimeLine = playDoorTimeLine;
        mapComponent.ActiveDoor();

        var nextMap = _mapDictionary[addressableKey];
        nextMap.SetActive(true);
        _currentMap = nextMap;

        mapData.MapAddressablesKey = addressableKey;
        LoadSceneManager.Instance.StartLoadingUICoroutine(false);
    }
}

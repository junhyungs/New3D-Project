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

        var startMapAddressables = mapData.MapAddressablesKey != null ?
            mapData.MapAddressablesKey : AddressablesKey.Map_Level_0;
        await LoadMapAsync(startMapAddressables);

        if(mapData.PlayerPosition != Vector3.zero &&
            mapData.PlayerRotation != Quaternion.identity)
            PlayerManager.Instance.EnablePlayer(mapData.PlayerPosition, mapData.PlayerRotation);    
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

    public async UniTask LoadMapAsync(string addressableKey, LinkedDoor linkedDoor = LinkedDoor.Default)
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
        mapData.MapAddressablesKey = addressableKey;

        if(nextMap.TryGetComponent(out Map mapCompnent))
        {
            mapCompnent.Initialize(mapData.ProgressDictionary);
            mapCompnent.LinkedDoor = linkedDoor;
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

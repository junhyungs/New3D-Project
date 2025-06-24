using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

[System.Serializable]
public class MapInfo
{
    [Header("MapObject")]
    public GameObject MapObject;
    [Header("MapName")]
    public MapName Name;
}

public class MapManager : MonoBehaviour
{
    [Header("MapInfo"), SerializeField]
    private MapInfo[] _mapInfos;
    private Dictionary<MapName, GameObject> _mapDictionary = new Dictionary<MapName, GameObject>();

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        LoadMap(MapName.Level_0);
    }

    private void Initialize()
    {
        foreach (var mapinfo in _mapInfos)
            if (mapinfo != null)
                _mapDictionary.Add(mapinfo.Name, mapinfo.MapObject);
    }

    public void LoadMap(MapName mapName)
    {
        UIManager.Instance.StartLoadingUI(true);

        var nextMap = _mapDictionary[mapName];
        if(nextMap == null)
            nextMap = LoadPrefab(mapName);

        nextMap.SetActive(true);
        UIManager.Instance.StartLoadingUI(false);
    }

    private GameObject LoadPrefab(MapName mapName)
    {
        var path = $"Map/{mapName.ToString()}";
        var gameObject = Resources.Load<GameObject>(path);

        var mapObject = Instantiate(gameObject);
        _mapDictionary.Add(mapName, mapObject);
        return mapObject;
    }
}

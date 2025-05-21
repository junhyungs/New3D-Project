using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Newtonsoft.Json.Linq;

public class ScreenResolution
{
    public ScreenResolution()
    {
        var dataList = DataManager.Instance.LoadResolutionData();
        _resolutions = GetResolution(dataList);
    }

    private Resolution[] _resolutions;
    public Resolution[] Resolutions => _resolutions;

    public Resolution GetCurrentResolution()
    {
        return new Resolution
        {
            width = Screen.width,
            height = Screen.height
        };
    }

    private Resolution[] GetResolution(List<(int,int)> dataList)
    {
        var resolutionArray = new Resolution[dataList.Count];
        for(int i = 0; i < resolutionArray.Length; i++)
        {
            resolutionArray[i].width = dataList[i].Item1;
            resolutionArray[i].height = dataList[i].Item2;
        }

        return resolutionArray;
    }

    public void ChangeResolution(int index)
    {
        var resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.Windowed);
    }
}

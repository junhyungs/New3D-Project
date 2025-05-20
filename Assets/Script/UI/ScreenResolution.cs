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
        var dataList = LoadResolutionData();
        _resolutions = GetResolution(dataList);
    }

    private Resolution[] _resolutions;
    public Resolution[] Resolutions => _resolutions;

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

    private List<(int,int)> LoadResolutionData()
    {
        var dataList = new List<(int, int)>();
        var jsonName = JsonData.New_3D_ScreenResolution.ToString();
        var jsonData = Resources.Load<TextAsset>($"JsonData/{jsonName}");

        JArray jArray = JArray.Parse(jsonData.text);
        foreach(var itme in jArray)
        {
            int width = ParseInt(itme["Width"]);
            int height = ParseInt(itme["Height"]);

            dataList.Add((width, height));
        }

        return dataList;
    }

    private int ParseInt(JToken jToken)
    {
        if(jToken == null || !int.TryParse(jToken.ToString(), out int value))
            return 0;

        return value;
    }

    public void ChangeResolution(int index)
    {
        var resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.Windowed);
    }
}

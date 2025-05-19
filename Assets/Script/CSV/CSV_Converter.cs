using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class CSV_Converter : MonoBehaviour
{
    [Header("FileName")]
    public string FileName;

    private const string DataPath = "CSV_Data";

    public void ParseCSVData()
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        var path = GetCSVPath();

        using(StreamReader reader = new StreamReader(path))
        {
            string header = reader.ReadLine();

            if (header == null)
                return;

            string[] headers = header.Split(',');

            while (!reader.EndOfStream)
            {
                string[] dataArray = reader.ReadLine().Split(",");
                Dictionary<string, string> rows = new Dictionary<string, string>();

                for(int i = 0; i < headers.Length; i++)
                {
                    //for는 headers.Length만큼 반복. dataArray.Length = 현재 읽어온 데이터길이.
                    //헤더는 3인데 현재 읽어온 데이터는 2인 경우에 에러를 방지하기 위해 
                    //dataArray.Length > i 보다 큰 경우(데이터 존재)에는 데이터를 딕셔너리에 넣고
                    //데이터가 없다면 ""처리.
                    rows[headers[i]] = dataArray.Length > i ? dataArray[i] : "";
                }

                dataList.Add(rows);
            }
        }

        string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);
        string saveDirectoryPath = Path.Combine(Application.dataPath, "Resources", "JsonData");

        if (!Directory.Exists(saveDirectoryPath))
        {
            Directory.CreateDirectory(saveDirectoryPath);
        }

        string savePath = Path.Combine(saveDirectoryPath, FileName + ".json");
        File.WriteAllText(savePath, jsonData);

        ClearFileName();
    }

    public void ClearFileName()
    {
        FileName = string.Empty;
    }

    public string GetCSVPath()
    {
        var path = Path.Combine(Application.dataPath, DataPath, FileName + ".csv").Replace("\\", "/");

        return path;
    }
}

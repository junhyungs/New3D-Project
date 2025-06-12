using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class CSV_Converter : MonoBehaviour
{
    [Header("FileName")]
    public string FileName;

    private const string DataPath = "CSV_Data";

    public void ParseCSVData()
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        var path = GetCSVPath();

        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true, //헤더를 읽기 위함. 첫 줄이 헤더라는것을 CsvReader에 전달.
            IgnoreBlankLines = true, //공백줄 무시.
            MissingFieldFound = null, //헤더는 있지만, 값이 없는 경우 에러 무시.(CSV가 완전하지 않으면 사용)
            BadDataFound = null, //CSV 오류 무시. (외부 파일 다룰 때 사용)
            HeaderValidated = null //헤더 필드가 매핑되지 않았을 경우 에러 무시. (Dictionary<string,string> 구조로 사용하는 경우 유용), (바로 클래스 매핑을 하지않는 경우)
        }))
        {
            bool IsRead = csv.Read() && csv.ReadHeader();

            if (!IsRead)
            {
                Debug.Log("CSV Read Fail");
                return;
            }
                
            while (csv.Read())
            {
                var row = new Dictionary<string, string>();
                foreach(var header in csv.HeaderRecord)
                {
                    row[header] = csv.GetField(header).Replace("\\n", "\n");
                }

                dataList.Add(row);
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

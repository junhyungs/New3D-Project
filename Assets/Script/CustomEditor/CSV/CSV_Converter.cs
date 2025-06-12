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
            HasHeaderRecord = true, //����� �б� ����. ù ���� �����°��� CsvReader�� ����.
            IgnoreBlankLines = true, //������ ����.
            MissingFieldFound = null, //����� ������, ���� ���� ��� ���� ����.(CSV�� �������� ������ ���)
            BadDataFound = null, //CSV ���� ����. (�ܺ� ���� �ٷ� �� ���)
            HeaderValidated = null //��� �ʵ尡 ���ε��� �ʾ��� ��� ���� ����. (Dictionary<string,string> ������ ����ϴ� ��� ����), (�ٷ� Ŭ���� ������ �����ʴ� ���)
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

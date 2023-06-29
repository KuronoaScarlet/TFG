using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVExporter
{
    private readonly IList<string> _dataArray;
    private readonly EntityType _enType;
    private readonly EntityClass _enClass;
    
    public CSVExporter(IList<string> data, EntityType e, EntityClass eC = EntityClass.None)
    {
        _dataArray = data;
        _enType = e;
        _enClass = eC;
    }
    public void Export()
    {
        Debug.Log($"Exporting {_enClass} {_enType}");
        
        string filePath = "";
        CreateDirectoryIfNecessary($"{Application.dataPath}/Exports");
        if(_enType == EntityType.Player)
        {
            CreateDirectoryIfNecessary($"{Application.dataPath}/Exports/{_enType}");
            filePath = $"{Application.dataPath}/Exports/{_enType}/{_enType}Stats.csv";
        }
        else if (_enType == EntityType.Enemy)
        {
            CreateDirectoryIfNecessary($"{Application.dataPath}/Exports/{_enType}");
            filePath = $"{Application.dataPath}/Exports/{_enType}/{_enClass}{_enType}Stats.csv";
        }
        
        StreamWriter sw = new StreamWriter(filePath);
    
        foreach (var t in _dataArray)
        {
            sw.WriteLine(t);
        }
        
        sw.Close();
    
        Debug.Log($"CSV exportado en: {filePath}");
    }

    private static void CreateDirectoryIfNecessary(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }
}

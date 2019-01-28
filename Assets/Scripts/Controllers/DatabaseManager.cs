using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    public static BuildingsData buildingsData;

    public List<Building> buildingObjects = new List<Building>();
    
    const string buildingsJson = "buildings.json";

    void OnValidate()
    {
        Assert.IsFalse(buildingObjects.Count == 0, "buildingObjects has no member.");
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadDatabase(Action onComplete)
    {
        StartCoroutine(LoadBuildingListJson(onComplete));
    }

    public IEnumerator LoadBuildingListJson(Action onComplete)
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, buildingsJson);
        
        var jsonData = "";
        if (filePath.Contains("://"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            jsonData = www.downloadHandler.text;
            print(jsonData);
            buildingsData = JsonUtility.FromJson<BuildingsData>(jsonData);
            onComplete();
        }
        else
        {
            jsonData = System.IO.File.ReadAllText(filePath);
            print(jsonData);
            buildingsData = JsonUtility.FromJson<BuildingsData>(jsonData);
            onComplete();
        }
    }

    public static BuildingData FindBuildingData(int id)
    {
        foreach(var data in buildingsData.buildings)
        {
            if (data.id == id)
                return data.Clone();
        }
        Debug.LogError("Can't find building data ");
        return null;
    }

    public Building FindBuildingObject(int id)
    {
        foreach(var building in buildingObjects)
        {
            if (building.dataId == id)
                return building;
        }
        Debug.LogError("Can't find building object ");
        return null;
    }

}

[System.Serializable]
public class BuildingsData
{
    public List<BuildingData> buildings;
}

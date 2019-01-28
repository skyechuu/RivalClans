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

    /// <summary>
    /// Request LoadBuildingListJson and forwards onComplete action.
    /// </summary>
    /// <param name="onComplete"></param>
    public void LoadDatabase(Action onComplete)
    {
        StartCoroutine(LoadBuildingListJson(onComplete));
    }

    /// <summary>
    /// Load building list from .json file. Calls onComplete after loading is done.
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Finds BuildingData with given ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Finds non-instanced Building with given ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

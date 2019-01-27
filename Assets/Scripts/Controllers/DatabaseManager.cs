using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    public List<Building> buildingObjects = new List<Building>();
    public static BuildingsData buildingsData;

    const string buildingsJson = "buildings.json";

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static IEnumerator LoadBuildingListJson()
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, buildingsJson);
        
        var jsonData = "";
        if (filePath.Contains("://"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            jsonData = www.downloadHandler.text;
            //print(jsonData);
            buildingsData = JsonUtility.FromJson<BuildingsData>(jsonData);
        }
        else
        {
            jsonData = System.IO.File.ReadAllText(filePath);
            //print(jsonData);
            buildingsData = JsonUtility.FromJson<BuildingsData>(jsonData);
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

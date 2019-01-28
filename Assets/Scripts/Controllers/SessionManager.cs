using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public struct BuildForSaleInfo
{
    public Resource buildCost;
    public int remaining;
}

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    public static Dictionary<int, Building> instancedBuildings;
    public static Dictionary<Building, BuildForSaleInfo> availableBuildings;
    public static Dictionary<BuildingCategory, int> buildingCounts;

    private SessionData data;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        DatabaseManager.instance.LoadDatabase(OnDatabaseReady);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SaveSession();
    }

    void OnDatabaseReady()
    {
        if (PlayerPrefs.GetInt("SESSION_AVAILABLE", 0) == 1)
        {
            GridManager.instance.OnStart();
            LoadSession();
        }
        else
        {
            GridManager.instance.OnStart(true);
            LoadDefault();
        }
    }

    public void SaveSession()
    {
        print("Save in progress...");
        SessionData data = new SessionData();
        data.savedBuildingCoordinates = new Dictionary<Coord, int>();
        foreach (var item in instancedBuildings)
        {
            data.savedBuildingCoordinates.Add(item.Value.GetCoord(), item.Value.dataId);
        }

        data.savedBuildingSaleInfos = new Dictionary<int, BuildForSaleInfo>();
        foreach(var item in availableBuildings)
        {
            data.savedBuildingSaleInfos.Add(item.Key.dataId, item.Value);
        }

        data.savedBuildingCounts = new Dictionary<int, int>();
        foreach(var item in buildingCounts)
        {
            data.savedBuildingCounts.Add((int)item.Key, item.Value);
        }

        data.savedResources = ResourcesManager.instance.GetTotalResources();
        
        var bf = new BinaryFormatter();
        var file = File.Create(Application.persistentDataPath + "/" + "game.session");
        bf.Serialize(file, data);
        file.Close();

        PlayerPrefs.SetInt("SESSION_AVAILABLE", 1);

        print("Session Saved.");
    }

    void LoadSession()
    {
        print("Load in progress...");
        var filePath = Path.Combine(Application.persistentDataPath, "game.session");
        var formatter = new BinaryFormatter();
        var file = File.Open(filePath, FileMode.Open);
        data = (SessionData)formatter.Deserialize(file);
        file.Close();

        instancedBuildings = new Dictionary<int, Building>();


        print("Load in progress... 1");
        foreach (var item in data.savedBuildingCoordinates)
        {
            var building = DatabaseManager.instance.FindBuildingObject(item.Value);
            if (building)
                BuildingManager.instance.Build(building, item.Key);
        }


        print("Load in progress... 2");
        availableBuildings = new Dictionary<Building, BuildForSaleInfo>();
        foreach (var item in DatabaseManager.instance.buildingObjects)
        {
            BuildForSaleInfo info = new BuildForSaleInfo();
            foreach (var infoItem in data.savedBuildingSaleInfos)
            {
                if (infoItem.Key == item.dataId)
                {
                    info = infoItem.Value;
                    break;
                }
                else
                {
                    info.buildCost = item.data.buildCost;
                    info.remaining = item.data.maxAmount;
                }
            }
            availableBuildings.Add(item, info);
        }


        print("Load in progress... 3");
        buildingCounts = new Dictionary<BuildingCategory, int>();
        foreach(var item in data.savedBuildingCounts)
        {
            buildingCounts.Add((BuildingCategory)item.Key, item.Value);
        }


        print("Load in progress... 4");
        ResourcesManager.instance.SetTotalResources(data.savedResources);

        print("Session Loaded.");
    }

    void LoadDefault()
    {
        buildingCounts = new Dictionary<BuildingCategory, int>();
        buildingCounts.Add(BuildingCategory.Category1, 0);
        buildingCounts.Add(BuildingCategory.Category2, 0);

        instancedBuildings = new Dictionary<int, Building>();

        LoadAvailableBuildings();
    }

    void LoadAvailableBuildings()
    {
        availableBuildings = new Dictionary<Building, BuildForSaleInfo>();
        foreach(var item in DatabaseManager.instance.buildingObjects)
        {
            BuildForSaleInfo info = new BuildForSaleInfo()
            {
                buildCost = item.data.buildCost,
                remaining = item.data.maxAmount
            };
            availableBuildings.Add(item, info);
        }
    }

    public static void AddBuildingInstance(Building building)
    {
        instancedBuildings.Add(building.gameObject.GetInstanceID(), building);
    }

    public static void RemoveBuildingInstance(Building building)
    {
        instancedBuildings.Remove(building.gameObject.GetInstanceID());
    }

    public static Dictionary<int, Building> GetInstancedBuildings()
    {
        return instancedBuildings;
    }

    public static void ChangeCategoryCount(BuildingCategory category, int value)
    {
        if (buildingCounts[category] + value <= 0)
            buildingCounts[category] = 0;
        buildingCounts[category] += value;
        
    }

    public Building FindBuildingWithInstanceID(int instanceID)
    {
        if (instancedBuildings.ContainsKey(instanceID))
            return instancedBuildings.Where(i => i.Key == instanceID).Select(s => s.Value) as Building;
        return null;
    }

    public void PrintInstancedBuildings()
    {
        string result = "";
        foreach (var item in instancedBuildings)
        {
            result += item.Key.ToString() + " : " + item.Value + "\n";
        }
        UnityEngine.Debug.Log(result);
    }

    public void ChangeBuildCost(Building building, Resource newCost)
    {
        BuildForSaleInfo info = availableBuildings[building];
        info.buildCost = newCost;
        availableBuildings[building] = info;
    }

    public void ChangeRemainingAmount(Building building, int value)
    {
        BuildForSaleInfo info = availableBuildings[building];
        info.remaining = value;
        availableBuildings[building] = info;
    }

    public Building FindNonInstancedByID(int id)
    {
        foreach(var item in availableBuildings)
        {
            if (item.Key.dataId == id)
                return item.Key;
        }
        return null;
    }

    
}

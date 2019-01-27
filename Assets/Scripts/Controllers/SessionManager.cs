using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    void Awake()
    {
        if (instance == null)
            instance = this;
        LoadDefault();
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

    public static Dictionary<int, Building> GetInstancedBuildings()
    {
        return instancedBuildings;
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

    public static void ChangeCategoryCount(BuildingCategory category, int value)
    {
        if (buildingCounts[category] + value <= 0)
            buildingCounts[category] = 0;
        buildingCounts[category] += value;
        
    }
    
}

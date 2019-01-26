using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    public static Dictionary<int, Building> instancedBuildings;

    public List<Building> availableBuildings;
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
    }

    public static void AddBuildingInstance(Building building)
    {
        instancedBuildings.Add(building.gameObject.GetInstanceID(), building);
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

}

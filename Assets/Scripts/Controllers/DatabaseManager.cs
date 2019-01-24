using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DatabaseManager
{
    public static Dictionary<int, Building> instancedBuildings;

    public DatabaseManager()
    {
        instancedBuildings = new Dictionary<int, Building>();
    }

    public void AddBuildingInstance(Building building)
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
        foreach(var item in instancedBuildings)
        {
            result += item.Key.ToString() + " : " + item.Value + "\n";
        }
        UnityEngine.Debug.Log(result);
    }
}

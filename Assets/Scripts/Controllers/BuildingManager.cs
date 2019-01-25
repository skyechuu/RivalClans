using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    public List<Building> availableBuildings;


    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public void Build(Building _building, Coord coord)
    {
        Building building = Object.Instantiate(_building, Tools.GetWorldPosition(coord, _building.GetSize()), Quaternion.identity, transform);
        building.coord = coord;
        SessionManager.instance.GetDatabaseManager().AddBuildingInstance(building);
        GridManager.instance.UpdateBuilding(building, UpdateType.NEW);
    }

    public void Build(Building _building)
    {
        Coord coord = GridManager.instance.FindRandomArea(_building);
        Building building = Object.Instantiate(_building, Tools.GetWorldPosition(coord, _building.GetSize()), Quaternion.identity, transform);
        building.coord = coord;
        SessionManager.instance.GetDatabaseManager().AddBuildingInstance(building);
        GridManager.instance.UpdateBuilding(building, UpdateType.NEW);
    }
    

}

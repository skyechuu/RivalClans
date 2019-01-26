using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;


    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public void Build(Building _building, Coord coord)
    {
        Building building = Object.Instantiate(_building, Tools.GetWorldPosition(coord, _building.GetSize()), Quaternion.identity, transform);
        building.data.coord = coord;
        SessionManager.AddBuildingInstance(building);
        GridManager.instance.UpdateBuilding(building, UpdateType.NEW);

    }

    public void Build(Building _building)
    {
        Coord coord = GridManager.instance.FindRandomArea(_building);
        Building building = Instantiate(_building, Tools.GetWorldPosition(coord, _building.GetSize()), Quaternion.identity, transform);
        building.data.coord = coord;
        SessionManager.AddBuildingInstance(building);
        SessionManager.buildingCounts[building.data.category]++;
        GridManager.instance.UpdateBuilding(building, UpdateType.NEW);
        _building.buildCost = GetBuildingCostWithInterest(building);
    }
    
    public Resource GetBuildingCostWithInterest(Building b)
    {
        var count = SessionManager.buildingCounts[b.data.category];
        return ApplyMultipleInterest(b.data.buildCost, GameConstants.BUILDING_COST_INTEREST, count);
    }

    private Resource ApplyMultipleInterest(Resource resource, int interestPercentage, int count)
    {
        if (count == 0)
            return resource;
        return ApplyInterest(ApplyMultipleInterest(resource, interestPercentage, count - 1), interestPercentage);
    }

    private Resource ApplyInterest(Resource resource, int interestPercentage)
    {
        int wood = resource.wood;
        int rock = resource.rock;
        int coin = resource.coin;

        wood += wood * interestPercentage / 100;
        rock += rock * interestPercentage / 100;
        coin += coin* interestPercentage / 100;

        return new Resource(wood, rock, coin);
    }
    

}

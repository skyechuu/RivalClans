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

    /// <summary>
    /// Build function for debug only, with given coordinates.
    /// </summary>
    /// <param name="_building"></param>
    /// <param name="coord"></param>
    public void Build(Building _building, Coord coord)
    {
        Building building = Object.Instantiate(_building, Tools.GetWorldPosition(coord, _building.GetSize()), Quaternion.identity, transform);
        building.SetCoord(coord);
        building.data = DatabaseManager.FindBuildingData(building.dataId);
        GridManager.instance.UpdateBuilding(building, UpdateType.NEW);
        SessionManager.AddBuildingInstance(building);
    }

    /// <summary>
    /// Builds given building to the grid.
    /// </summary>
    /// <param name="_building">Non-instanced building selected by build menu.</param>
    public void Build(Building _building)
    {
        Coord coord = GridManager.instance.FindRandomArea(_building);
        Building building = Instantiate(_building, Tools.GetWorldPosition(coord, _building.GetSize()), Quaternion.identity, transform);
        building.SetCoord(coord);
        building.data = DatabaseManager.FindBuildingData(building.dataId);
        building.data.buildCost =  SessionManager.availableBuildings[_building].buildCost;

        GridManager.instance.UpdateBuilding(building, UpdateType.NEW);
        SessionManager.AddBuildingInstance(building);
        SessionManager.ChangeCategoryCount(building.GetCategory(), 1);
        ApplyInterestToBuildingCategory(building.GetCategory());
        ChangeRemainingOfBuilding(_building, -1);

        InputManager.instance.OnSelectBuilding(building);
    }

    /// <summary>
    /// Remove given building from scene.
    /// </summary>
    /// <param name="building"></param>
    public void RemoveBuilding(Building building)
    {
        GridManager.instance.UpdateBuilding(building, UpdateType.REMOVE);
        SessionManager.RemoveBuildingInstance(building);
        var _building = SessionManager.instance.FindNonInstancedByID(building.dataId);
        ChangeRemainingOfBuilding(_building, 1);
        SessionManager.ChangeCategoryCount(building.GetCategory(), -1);
        ApplyInterestToBuildingCategory(building.GetCategory());
        Destroy(building.gameObject);
    }

    /// <summary>
    /// Change remaining amount of given building, by value.
    /// </summary>
    /// <param name="building"></param>
    /// <param name="value">Value of change</param>
    private void ChangeRemainingOfBuilding(Building building, int value)
    {
        int remaining = SessionManager.availableBuildings[building].remaining;
        SessionManager.instance.ChangeRemainingAmount(building, remaining + value);
    }

    /// <summary>
    /// Apply interest rate to given category after build.
    /// </summary>
    /// <param name="category"></param>
    private void ApplyInterestToBuildingCategory(BuildingCategory category)
    {
        List<Building> markForChange = new List<Building>();

        foreach (var item in SessionManager.availableBuildings)
        {
            if ((BuildingCategory)item.Key.data.category == category)
                markForChange.Add(item.Key);
        }

        foreach(var building in markForChange)
            SessionManager.instance.ChangeBuildCost(building, CalculateBuildingCostWithInterest(building));
        
    }
    
    /// <summary>
    /// Calcualtes building cost with interest.
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    private Resource CalculateBuildingCostWithInterest(Building building)
    {
        var count = SessionManager.buildingCounts[(BuildingCategory)building.data.category];
        return ApplyMultipleInterest(building.data.buildCost, GameConstants.BUILDING_COST_INTEREST, count);
    }

    /// <summary>
    /// Recursive function of apply interest in to given build cost.
    /// </summary>
    /// <param name="buildCost">Base build cost of building</param>
    /// <param name="interestPercentage">Interest rate by percentage</param>
    /// <param name="count">Amount of interest that will apply</param>
    /// <returns></returns>
    private Resource ApplyMultipleInterest(Resource buildCost, int interestPercentage, int count)
    {
        if (count <= 0)
            return buildCost;
        return ApplyInterest(ApplyMultipleInterest(buildCost, interestPercentage, count - 1), interestPercentage);
    }

    /// <summary>
    /// Apply interest to given build cost.
    /// </summary>
    /// <param name="buildCost">Base build cost of building</param>
    /// <param name="interestPercentage">Interest rate by percentage</param>
    /// <returns></returns>
    private Resource ApplyInterest(Resource buildCost, int interestPercentage)
    {
        int wood = buildCost.wood;
        int rock = buildCost.rock;
        int coin = buildCost.coin;

        wood += Mathf.CeilToInt(wood * interestPercentage / 100f);
        rock += Mathf.CeilToInt(rock * interestPercentage / 100f);
        coin += Mathf.CeilToInt(coin * interestPercentage / 100f);

        return new Resource(wood, rock, coin);
    }

}

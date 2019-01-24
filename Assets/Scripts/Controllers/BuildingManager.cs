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

    void Start ()
    {
        //StartCoroutine(SeedData());
    }
	
	void Update () {
		
	}

    IEnumerator SeedData()
    {
        yield return new WaitForSeconds(.25f);
        Build(availableBuildings[1], new Coord { x = 4, y = 5 });
        Build(availableBuildings[0], new Coord { x = 3, y = 3 });
        Build(availableBuildings[0], new Coord { x = 1, y = 2 });
    }
    
    public void Build(Building _building, Coord coord)
    {
        Building building = Instantiate(_building, Tools.GetWorldPosition(coord, _building.GetSize()), Quaternion.identity, transform);
        building.coord = coord;
        SessionManager.instance.GetDatabaseManager().AddBuildingInstance(building);
        GridManager.instance.UpdateBuilding(building, UpdateType.NEW);
    }
}

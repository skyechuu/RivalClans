using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    public List<Building> availableBuildings;
    public List<Building> currentBuildings = new List<Building>();

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
        yield return new WaitForSeconds(1);
        Build(availableBuildings[0], GridManager.instance.GetGrid(0, 0));
        Build(availableBuildings[1], GridManager.instance.GetGrid(4, 5));
    }
    
    public void Build(Building building, Grid grid)
    {
        var position = grid.transform.position;
        if (building.category == BuildingCategory.Category2)
            position += new Vector3(0.5f, 0, 0.5f); // TODO : Calculate this by searching 2x2 available grid area
        Building b = Instantiate(building, position, Quaternion.identity, transform);
        currentBuildings.Add(b);
        OccupyGridsUnderBuilding(b);
    }

    public void MoveBuilding(Building building, Grid grid, Vector3 buildOffset)
    {
        var position = grid.transform.position + buildOffset;
        building.transform.position = position;
        OccupyGridUnderCurrentBuildings();
    }
    
    List<Grid> FindGridsUnderBuilding(Building building)
    {
        var grids = new List<Grid>();
        if(building.category == BuildingCategory.Category2)
        {
            for(int i = -1; i<=1; i+=2)
            {
                for (int j = -1; j<=1; j+=2)
                {
                    var grid = GridManager.instance.GetGrid(building.transform.position.x + 0.5f * i, building.transform.position.z + 0.5f * j);
                    grids.Add(grid);
                }
            }
        }
        else
        {
            grids.Add(GridManager.instance.GetGrid(building.transform.position.x, building.transform.position.z));
        }

        return grids;
    }

    void OccupyGridsUnderBuilding(Building building)
    {
        var grids = FindGridsUnderBuilding(building);
        foreach(Grid grid in grids)
        {
            grid.building = building;
            grid.SetOccupied(true);
        }
    }

    void OccupyGridUnderCurrentBuildings()
    {
        GridManager.instance.ClearGrid();
        foreach (Building building in currentBuildings)
        {
            OccupyGridsUnderBuilding(building);
        }
    }


}

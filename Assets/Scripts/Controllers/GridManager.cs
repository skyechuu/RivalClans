using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager instance;

    [Header("Settings")]
    [Range(0, 50)]
    [SerializeField] int percentageOfInitiallyOccupiedGrids = 10;
    [SerializeField] GameObject gridPrefab;
    
    int[,] grids;

    // for visualization
    Grid[,] gridMap;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

	void Start () {
        CreateGridMap();
        InitiallyOccupyGrids();
        
    }

    void CreateGridMap()
    {
        grids = new int[GameConstants.GRID_DIMENSION_X, GameConstants.GRID_DIMENSION_Y];
        gridMap = new Grid[GameConstants.GRID_DIMENSION_X, GameConstants.GRID_DIMENSION_Y];
        for (int i = 0; i < GameConstants.GRID_DIMENSION_X; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                Vector3 position = new Vector3(-GameConstants.GRID_DIMENSION_X / 2 + 0.5f + i, 0, -GameConstants.GRID_DIMENSION_Y / 2 + 0.5f + j);
                GameObject go = Instantiate(gridPrefab, position, Quaternion.identity, transform);
                go.transform.name = "Grid[" + i + "," + j + "]";
                gridMap[i, j] = go.GetComponent<Grid>();
                grids[i, j] = 0;
            }
        }
        GetComponent<BoxCollider>().size = new Vector3(GameConstants.GRID_DIMENSION_X * GameConstants.GRID_UNIT, 0.1f, GameConstants.GRID_DIMENSION_Y * GameConstants.GRID_UNIT);
    }

    public void VisualizeGridMap()
    {
        for (int i = 0; i < GameConstants.GRID_DIMENSION_X; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                if (grids[i, j] == 0)
                    gridMap[i, j].SetColor(Color.white);
                else
                {
                    gridMap[i, j].SetColor(Color.yellow);
                }
            }
        }
    }

    public void VisualizeGridMap(Coord position, int buildingSize, int instance)
    {
        ClearGrid();
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                if(Tools.IsIndexInGridDimensions(i, j))
                {
                    if ((grids[i, j] == 0 || grids[i, j] == instance) && Tools.IsIndexInGridDimensions(i,j))
                        gridMap[i, j].SetColor(Color.green);
                    else
                        gridMap[i, j].SetColor(Color.red);
                }
            }
        }
    }

    public void ClearGrid()
    {
        for (int i = 0; i < GameConstants.GRID_DIMENSION_X; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                gridMap[i, j].SetColor(Color.white);
            }
        }
    }

    void InitiallyOccupyGrids()
    {
        if (percentageOfInitiallyOccupiedGrids == 0)
            return;

        var initialOccupyAmount = GameConstants.GRID_DIMENSION_X * GameConstants.GRID_DIMENSION_Y * percentageOfInitiallyOccupiedGrids / 100;
        var occupied = 0;
        while (occupied != initialOccupyAmount)
        {
            var buildingIndex = (initialOccupyAmount - occupied > 3) ? UnityEngine.Random.Range(0, BuildingManager.instance.availableBuildings.Count) : 0;
            var building = BuildingManager.instance.availableBuildings[buildingIndex];
            Coord coord = FindRandomArea(building);
            
            BuildingManager.instance.Build(building, coord);
            occupied += (int)Mathf.Pow(building.GetSize(), 2);
        }
    }

    public Coord FindRandomArea(Building b)
    {
        int x, y;
        x = UnityEngine.Random.Range(0, GameConstants.GRID_DIMENSION_X);
        y = UnityEngine.Random.Range(0, GameConstants.GRID_DIMENSION_Y);
        if (!IsAreaAvailable(new Coord(x, y), b.GetSize(), 0)){
            return FindRandomArea(b);
        }
        return new Coord(x, y);
    }

    public int GetInstanceFromGrid(float posX, float posY)
    {
        int x = (int)(posX - (-GameConstants.GRID_DIMENSION_X / 2 + 0.5f));
        int y = (int)(posY - (-GameConstants.GRID_DIMENSION_Y / 2 + 0.5f));
        return grids[x, y];
    }

    public int GetInstanceFromGrid(int x, int y)
    {
        return grids[x, y];
    }

    public bool IsAreaAvailable(Coord position, int buildingSize, int instance)
    {
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                if (i < 0 || i >= GameConstants.GRID_DIMENSION_X || j < 0 || j >= GameConstants.GRID_DIMENSION_Y)
                {
                    return false;
                }
                if (grids[i, j] != 0 && grids[i, j] != instance)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void UpdateBuilding(Building building, UpdateType updateType)
    {
        if(updateType == UpdateType.NEW)
        {
            AddBuilding(building);
        }
        else if(updateType == UpdateType.REMOVE)
        {
            RemoveBuilding(building);
        }
        else if (updateType == UpdateType.CHANGE)
        {
            MoveBuilding(building);
        }
        Debug.LogWarning("Building "+building.name+" (" + building.GetInstanceID() + ") updated. Update Method: " + updateType.ToString());
        VisualizeGridMap();
    }

    private void AddBuilding(Building building)
    {
        var position = building.coord;
        var buildingSize = building.GetSize();
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                grids[i, j] = building.gameObject.GetInstanceID();
                
            }
        }
    }

    private void RemoveBuilding(Building building)
    {
        var position = building.coord;
        var buildingSize = building.GetSize();
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                grids[i, j] = 0;
            }
        }
    }

    private void MoveBuilding(Building building)
    {
        for (int i = 0; i < GameConstants.GRID_DIMENSION_Y; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                if(grids[i, j] == building.gameObject.GetInstanceID())
                    grids[i, j] = 0;
            }
        }
        AddBuilding(building);
    }

    public Building GetBuilding(int instanceID)
    {
        for (int i = 0; i < GameConstants.GRID_DIMENSION_X; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                if (grids[i, j] == instanceID)
                    return SessionManager.instance.GetDatabaseManager().FindBuildingWithInstanceID(instanceID);
            }
        }
        return null;
    }
}

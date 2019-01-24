using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager instance;

    [Header("Settings")]
    [Range(0, 100)]
    [SerializeField] int percentageOfInitiallyOccupiedGrids = 10;
    [SerializeField] GameObject gridPrefab;
    
    [SerializeField] int[,] grids;
    Grid[,] gridMap;
    


   
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

	void Start () {
        CreateGridMap();
        //InitiallyOccupyGrids();
        
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
    }

    void VisualizeGridMap()
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
        for (int i = 0; i < GameConstants.GRID_DIMENSION_X; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                gridMap[i, j].SetColor(Color.white);
                
            }
        }
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                if (grids[i, j] == 0 || grids[i, j] == instance)
                    gridMap[i, j].SetColor(Color.green);
                else
                    gridMap[i, j].SetColor(Color.red);
            }
        }
    }

    void InitiallyOccupyGrids()
    {
        var initialOccupyAmount = GameConstants.GRID_DIMENSION_X * GameConstants.GRID_DIMENSION_Y * percentageOfInitiallyOccupiedGrids / 100;
        for (int i = 0; i < initialOccupyAmount; i++)
        {
            int randomX, randomY;
            do
            {
                randomX = UnityEngine.Random.Range(0, GameConstants.GRID_DIMENSION_X - 1);
                randomY = UnityEngine.Random.Range(0, GameConstants.GRID_DIMENSION_Y - 1);
            }
            while (grids[randomX, randomY] != 0);
            //just set occupy later
            //BuildingManager.instance.Build(BuildingManager.instance.availableBuildings[1], grids[randomX, randomY]);
            //grids[randomX, randomY].SetOccupied(true);
        }
    }

    public void ClearGrid()
    {
        throw new NotImplementedException();
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
        //print(position.ToString());
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
        var position = building.coord;
        var buildingSize = building.GetSize();
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
        var position = building.coord;
        var buildingSize = building.GetSize();
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

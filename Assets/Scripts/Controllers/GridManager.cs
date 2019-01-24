using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager instance;

    [Header("Settings")]
    [SerializeField] int gridDimensionX = 20;
    [SerializeField] int gridDimensionY = 20;
    [Range(0, 100)]
    [SerializeField] int percentageOfInitiallyOccupiedGrids = 10;
    [SerializeField] GameObject gridPrefab;


    Grid[,] grids;

    private readonly int[,] allNeighbours = 
        new int[8, 2] 
        {
            {0,1},
            {1,1},
            {1,0},
            {1,-1},
            {0,-1},
            {-1,-1},
            {-1,0},
            {-1,1}
        };

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
        grids = new Grid[gridDimensionX,gridDimensionY];
        for (int i = 0; i < gridDimensionX; i++)
        {
            for (int j = 0; j < gridDimensionY; j++)
            {
                Vector3 gridPosition = new Vector3(-gridDimensionX / 2 + 0.5f + i, 0, -gridDimensionY / 2 + 0.5f + j);
                GameObject go = Instantiate(gridPrefab, gridPosition, Quaternion.identity, transform);
                go.transform.name = "Grid[" + i + "," + j + "]";
                grids[i, j] = go.GetComponent<Grid>();
                go.GetComponent<Grid>().x = i;
                go.GetComponent<Grid>().y = j;
            }
        }
    }

    private void InitiallyOccupyGrids()
    {
        var initialOccupyAmount = gridDimensionX * gridDimensionY * percentageOfInitiallyOccupiedGrids / 100;
        for (int i = 0; i < initialOccupyAmount; i++)
        {
            int randomX, randomY;
            do
            {
                randomX = UnityEngine.Random.Range(0, gridDimensionX - 1);
                randomY = UnityEngine.Random.Range(0, gridDimensionY - 1);
            }
            while (grids[randomX, randomY].occupied);
            //just set occupy later
            BuildingManager.instance.Build(BuildingManager.instance.availableBuildings[1], grids[randomX, randomY]);
            //grids[randomX, randomY].SetOccupied(true);
        }
    }

    public void ClearGrid()
    {
        foreach(Grid grid in grids)
        {
            grid.building = null;
            grid.SetOccupied(false);
        }
    }

    public Grid GetGrid(float posX, float posY)
    {
        int x = (int)(posX - (-gridDimensionX / 2 + 0.5f));
        int y = (int)(posY - (-gridDimensionY / 2 + 0.5f));
        return grids[x, y];
    }

    public Grid GetGrid(int x, int y)
    {
        return grids[x, y];
    }

    public Grid GetNeighbour(Grid center, int x, int y)
    {
        if(center.x + x < gridDimensionX && center.x + x >= 0 && center.y + y < gridDimensionY && center.y + y >= 0)
            return grids[center.x + x, center.y + y];
        return null;
    }
    
    public Grid GetTopNeighbour(Grid center)
    {
        return GetNeighbour(center, 0, 1);
    }

    public Grid GetBottomNeighbour(Grid center)
    {
        return GetNeighbour(center, 0, -1);
    }

    public Grid GetLeftNeighbour(Grid center)
    {
        return GetNeighbour(center, -1, 0);
    }

    public Grid GetRightNeighbour(Grid center)
    {
        return GetNeighbour(center, 1, 0);
    }

    public List<Grid> GetTwoByTwoGridArea(Grid center)
    {
        if (center.occupied)
            return null;

        List<Grid> grids = new List<Grid>();
        int startIndex = 0;
        for(int i = 0; i<startIndex+3; i++)
        {
            int x = allNeighbours[i%(allNeighbours.Length/2), 0];
            int y = allNeighbours[i%(allNeighbours.Length/2), 1];
            Grid neighbour = GetNeighbour(center, x, y);
            //print("start i: "+startIndex+" x:" + x + " y:" + y + " result:"+neighbour);
            if (neighbour == null || neighbour.occupied)
            {
                startIndex += 2;
                i = startIndex-1;
                grids.Clear();
                if (startIndex > 6)
                    return null;
            }
            else
            {
                grids.Add(neighbour);
                if(grids.Count == 3)
                {
                    grids.Add(center);
                    return grids;
                }
            }
        }
        return null;
    }

    public void OccupyArea(List<Grid> area)
    {
        if (area != null)
        {
            foreach (Grid grid in area)
            {
                grid.SetOccupied(true);
            }
        }
    }

}

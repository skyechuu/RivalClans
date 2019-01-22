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
            BuildingManager.instance.Build(BuildingManager.instance.availableBuildings[1], grids[randomX, randomY]);
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

    public Grid GetNeighbour(Grid root, int x, int y)
    {
        if(root.x + x <= gridDimensionX && root.x + x >= 0 && root.y + y <= gridDimensionY && root.y + y >= 0)
            return grids[root.x + x, root.y + y];
        return null;
    }
    
    public Grid GetTopNeighbour(Grid root)
    {
        return GetNeighbour(root, 0, 1);
    }

    public Grid GetBottomNeighbour(Grid root)
    {
        return GetNeighbour(root, 0, -1);
    }

    public Grid GetLeftNeighbour(Grid root)
    {
        return GetNeighbour(root, -1, 0);
    }

    public Grid GetRightNeighbour(Grid root)
    {
        return GetNeighbour(root, 1, 0);
    }

}

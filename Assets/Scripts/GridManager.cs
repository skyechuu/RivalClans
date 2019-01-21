using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public Grid[,] grids;

    [SerializeField] int gridDimensionX = 20;
    [SerializeField] int gridDimensionY = 20;
    [SerializeField] GameObject gridPrefab;

	void Start () {
        CreateGridMap();
	}
	
	void Update () {
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
                grids[i, j] = go.GetComponent<Grid>();
            }
        }
    }
}

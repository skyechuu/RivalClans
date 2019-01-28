using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class GridManager : MonoBehaviour {

    public static GridManager instance;

    [Header("Settings")]
    [Range(0, 50)]
    [SerializeField] int percentageOfInitiallyOccupiedGrids = 10;
    [SerializeField] GameObject gridPrefab;
    
    int[,] grids;

    // for visualization
    Grid[,] gridMap;

    void OnValidate()
    {
        Assert.IsNotNull(gridPrefab, "gridPrefab is set to null.");
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void OnStart(bool initiallyOccupyGrids = false)
    {
        CreateGridMap();
        if(initiallyOccupyGrids)
            StartCoroutine(RequestInitiallyOccupyGrids());
    }

    IEnumerator RequestInitiallyOccupyGrids()
    {
        yield return new WaitForSeconds(0.5f);
        InitiallyOccupyGrids();
    }

    /// <summary>
    /// Creates grid map with given dimensions. Dimensions declared at GameConstants.
    /// </summary>
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
        //GetComponent<BoxCollider>().size = new Vector3(GameConstants.GRID_DIMENSION_X * GameConstants.GRID_UNIT, 0.1f, GameConstants.GRID_DIMENSION_Y * GameConstants.GRID_UNIT);
    }

    /// <summary>
    /// Visualize the grid map with colors
    /// </summary>
    /// <param name="position">Building coordinate</param>
    /// <param name="buildingSize">Building size. Eg: buildingsize=2 means 2x2 grid are used by building.</param>
    /// <param name="instance">Building game object instance ID</param>
    public void VisualizeGridMap(Coord position, int buildingSize, int instance)
    {
        ClearGrid();
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                if(Tools.IsIndexInGridDimensions(i, j))
                {
                    if ((grids[i, j] == 0 || grids[i, j] == instance))
                        gridMap[i, j].SetColor(Color.green);
                    else
                        gridMap[i, j].SetColor(Color.red);
                }
            }
        }
    }
    /// <summary>
    /// Visualize the grid map with colors
    /// </summary>
    /// <param name="position">Building coordinate</param>
    /// <param name="buildingSize">Building size. Eg: buildingsize=2 means 2x2 grid are used by building.</param>
    /// <param name="instance">Building game object instance ID</param>
    /// <param name="c">Different color</param>
    public void VisualizeGridMap(Coord position, int buildingSize, int instance, Color c)
    {
        ClearGrid();
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                if (Tools.IsIndexInGridDimensions(i, j))
                {
                    if ((grids[i, j] == 0 || grids[i, j] == instance))
                        gridMap[i, j].SetColor(c);
                    else
                        gridMap[i, j].SetColor(Color.red);
                }
            }
        }
    }

    /// <summary>
    /// Clears grid to its default color.
    /// </summary>
    public void ClearGrid()
    {
        for (int i = 0; i < GameConstants.GRID_DIMENSION_X; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                gridMap[i, j].SetColor(GameConstants.DEFAULT_GRID_COLOR);
            }
        }
    }

    /// <summary>
    /// Initially occupy grids with objects. Objects used in that function are buildings.
    /// Assumed DatabaseManager.instance.buildingObjects[0] is a 1x1 object.
    /// </summary>
    void InitiallyOccupyGrids()
    {
        if (percentageOfInitiallyOccupiedGrids == 0)
            return;

        var initialOccupyAmount = GameConstants.GRID_DIMENSION_X * GameConstants.GRID_DIMENSION_Y * percentageOfInitiallyOccupiedGrids / 100;
        var occupied = 0;
        while (occupied <= initialOccupyAmount)
        {
            var buildingIndex = (initialOccupyAmount - occupied > 3) ? UnityEngine.Random.Range(0, DatabaseManager.instance.buildingObjects.Count) : 0;   // 0 = index where 1x1 building stored
            var building = DatabaseManager.instance.buildingObjects[buildingIndex];
            building.data = DatabaseManager.FindBuildingData(building.dataId);
            Coord coord = FindRandomArea(building);
            BuildingManager.instance.Build(building, coord);
            occupied += (int)Mathf.Pow(building.GetSize(), 2);
        }
    }

    /// <summary>
    /// Finds random and free grid area for given building
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public Coord FindRandomArea(Building building)
    {
        int x, y;
        x = UnityEngine.Random.Range(0, GameConstants.GRID_DIMENSION_X);
        y = UnityEngine.Random.Range(0, GameConstants.GRID_DIMENSION_Y);
        if (!IsAreaAvailable(new Coord(x, y), building.GetSize(), 0)){
            return FindRandomArea(building);
        }
        return new Coord(x, y);
    }

    /// <summary>
    /// Gets instance by given x and y world positions from grid.
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public int GetInstanceFromGrid(float posX, float posY)
    {
        int x = (int)(posX - (-GameConstants.GRID_DIMENSION_X / 2 + 0.5f));
        int y = (int)(posY - (-GameConstants.GRID_DIMENSION_Y / 2 + 0.5f));
        return grids[x, y];
    }

    /// <summary>
    /// Gets instance by given x and y coordinates from grid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetInstanceFromGrid(int x, int y)
    {
        return grids[x, y];
    }

    /// <summary>
    /// Checks if area by given coord and building size is available.
    /// </summary>
    /// <param name="position">Building coordinate</param>
    /// <param name="buildingSize">Building size. Eg: buildingsize=2 means 2x2 grid are used by building.</param>
    /// <param name="instance">Building instance.</param>
    /// <returns></returns>
    public bool IsAreaAvailable(Coord position, int buildingSize, int instance)
    {
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                if (!Tools.IsIndexInGridDimensions(i, j))
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

    /// <summary>
    /// Checks if given area is in grid dimensions.
    /// </summary>
    /// <param name="position">Building coordinate.</param>
    /// <param name="buildingSize">Building size. Eg: buildingsize=2 means 2x2 grid are used by building.</param>
    /// <returns></returns>
    public bool IsAreaInGrid(Coord position, int buildingSize)
    {
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                if (!Tools.IsIndexInGridDimensions(i, j))
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Updates building information depending on UpdateType
    /// </summary>
    /// <param name="building"></param>
    /// <param name="updateType"></param>
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
            SessionManager.instance.SaveSession();
        }
        //Debug.LogWarning("Building "+building.name+" (" + building.GetInstanceID() + ") updated. Update Method: " + updateType.ToString());
        
    }

    /// <summary>
    /// Adds building instance to grid.
    /// </summary>
    /// <param name="building"></param>
    private void AddBuilding(Building building)
    {
        var position = building.GetCoord();
        var buildingSize = building.GetSize();
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                grids[i, j] = building.gameObject.GetInstanceID();
            }
        }
    }

    /// <summary>
    /// Removes building instance from grid.
    /// </summary>
    /// <param name="building"></param>
    private void RemoveBuilding(Building building)
    {
        var position = building.GetCoord();
        var buildingSize = building.GetSize();
        for (int i = position.x; i < position.x + buildingSize; i++)
        {
            for (int j = position.y; j < position.y + buildingSize; j++)
            {
                grids[i, j] = 0;
            }
        }
    }

    /// <summary>
    /// Moves building instance on grid.
    /// </summary>
    /// <param name="building"></param>
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

    /// <summary>
    /// Get building instance from grid with given ID.
    /// </summary>
    /// <param name="instanceID"></param>
    /// <returns></returns>
    public Building GetBuilding(int instanceID)
    {
        for (int i = 0; i < GameConstants.GRID_DIMENSION_X; i++)
        {
            for (int j = 0; j < GameConstants.GRID_DIMENSION_Y; j++)
            {
                if (grids[i, j] == instanceID)
                    return SessionManager.instance.FindBuildingWithInstanceID(instanceID);
            }
        }
        return null;
    }
}

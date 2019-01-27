using UnityEngine;


public class Building : MonoBehaviour, IMoveable {

    public BuildingData data;
    public int dataId;

    private BuildingState state;
    private ResourceType currentResourceType;
    private Coord coord;

    Coord oldCoord;
    float nextTick = 2;

    void Start()
    {
        GetNextResourceType();
    }
    
    void Update()
    {
        HandleProduce();
    }

    void HandleProduce()
    {
        if (state != BuildingState.CONSTRUCTION)
        {
            if(Time.time > nextTick)
            {
                ProduceResource();
                nextTick = Time.time + data.produceRateInSeconds;
                GetNextResourceType();
            }
        }
    }

    void GetNextResourceType()
    {
        ResourceType type;
        if ((BuildingCategory)data.category == BuildingCategory.Category2)
            type = ResourceType.COIN;
        else
            type = SelectRandomResourceType();
        currentResourceType = type;
    }

    ResourceType SelectRandomResourceType()
    {
        int random = Random.Range(0, 3);
        return (ResourceType) random;
    }

    void ProduceResource()
    {
        int value = data.produces;
        
        ResourcesManager.instance.GetTotalResources().AddResource(value, currentResourceType);
    }

    public void OnMoveStarted()
    {
        if(state != BuildingState.MOVE)
        {
            oldCoord = coord;
            state = BuildingState.MOVE;
        }
    }

    public void OnMove(Vector3 position)
    {
        if(position != transform.position)
        {
            Coord _coord = Tools.GetGridCoord(position, GetSize());
            bool available = GridManager.instance.IsAreaInGrid(_coord, GetSize());
            if (available)
            {
                SetPosition(_coord);
                GridManager.instance.VisualizeGridMap(coord, GetSize(), gameObject.GetInstanceID());
            }
        }
    }

    public void OnMoveEnded()
    {
        if (!oldCoord.Equals(coord))
        {
            bool available = GridManager.instance.IsAreaAvailable(coord, GetSize(), gameObject.GetInstanceID());
            if (available)
            {
                GridManager.instance.UpdateBuilding(this, UpdateType.CHANGE);
                state = BuildingState.IDLE;
                GridManager.instance.ClearGrid();
            }
        }
        else
        {
            state = BuildingState.IDLE;
            GridManager.instance.VisualizeGridMap(coord, GetSize(), gameObject.GetInstanceID());
        }
    }

    public void OnCancelMove()
    {
        state = BuildingState.IDLE;
        SetPosition(oldCoord);
        GridManager.instance.VisualizeGridMap(coord, GetSize(), gameObject.GetInstanceID());
    }

    public void SetPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
        coord = Tools.GetGridCoord(worldPosition, GetSize());
    }

    public void SetPosition(Coord _coord)
    {
        transform.position = Tools.GetWorldPosition(_coord, GetSize());
        coord = _coord;
    }

    public int GetSize()
    {
        switch ((BuildingCategory)data.category)
        {
            case BuildingCategory.Category1:
                return 1;   // 1x1
            case BuildingCategory.Category2:
                return 2;   // 2x2
            default:
                Debug.LogError("("+gameObject.GetInstanceID()+") Building.GetSize() ");
                return 0;
        }
    }

    public BuildingState GetState()
    {
        return state;
    }

    public Coord GetCoord()
    {
        return coord;
    }

    public void SetCoord(Coord _coord)
    {
        coord = _coord;
    }

    public BuildingCategory GetCategory()
    {
        return (BuildingCategory)data.category;
    }

    public ResourceType GetCurrentResourceType()
    {
        return currentResourceType;
    }
}

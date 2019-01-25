using UnityEngine;

public class Building : MonoBehaviour, IMoveable {

    public BuildingCategory category;
    public BuildingState state;
    public string buildingName;
    public Resource buildCost;
    public Resource produces;
    public Coord coord;

    private Coord oldCoord;

    private float cooldown = 10;
    private float nextTick = 10;
    
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
                nextTick = Time.time + cooldown;
            }
        }
    }

    ResourceType SelectRandomResourceType()
    {
        int random = Random.Range(0, 3);
        return (ResourceType) random;
    }

    void ProduceResource()
    {
        ResourceType type;
        if(category == BuildingCategory.Category2)
            type = ResourceType.COIN;
        else
            type = SelectRandomResourceType();

        int value = 0;
        switch (type)
        {
            case ResourceType.COIN:
                value = produces.coin;
                break;
            case ResourceType.ROCK:
                value = produces.rock;
                break;
            case ResourceType.WOOD:
                value = produces.wood;
                break;
            default:
                break;
        }
        ResourcesManager.instance.GetTotalResources().AddResource(value, type);
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
        switch (category)
        {
            case BuildingCategory.Category1:
                return 1;   // 1x1
            case BuildingCategory.Category2:
                return 2;   // 2x2
            default:
                return 0;
        }
    }

}

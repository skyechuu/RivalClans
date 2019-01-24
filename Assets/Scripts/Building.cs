using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour, IMoveable {

    public BuildingCategory category;
    public BuildingState buildingState;
    public string buildingName;
    public Resource buildCost;
    public Resource produces;
    public Coord coord;

    private Coord oldCoord;

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

    public void OnMoveStarted()
    {
        oldCoord = coord;
        buildingState = BuildingState.MOVE;
    }

    public void OnMove(Vector3 position)
    {
        if(position != transform.position)
        {
            SetPosition(Tools.GetGridPosition(position, GetSize()));
            GridManager.instance.VisualizeGridMap(coord, GetSize(), gameObject.GetInstanceID());
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
                buildingState = BuildingState.IDLE;
            }
        }
    }

    public void OnCancelMove()
    {
        buildingState = BuildingState.IDLE;
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

}

﻿using UnityEngine;

public static class Tools
{
    public static Vector3 GetGridPosition(Vector3 position, int buildingSize)
    {
        float x = Mathf.Floor(position.x / GameConstants.GRID_UNIT) * GameConstants.GRID_UNIT + buildingSize / 2.0f;
        float z = Mathf.Floor(position.z / GameConstants.GRID_UNIT) * GameConstants.GRID_UNIT + buildingSize / 2.0f;
        return new Vector3(x, 0, z);
    }

    public static Vector3 GetWorldPosition(Coord coord, int buildingSize)
    {
        return new Vector3(-GameConstants.GRID_DIMENSION_X / 2 + (buildingSize / 2.0f) + coord.x, 0, -GameConstants.GRID_DIMENSION_Y / 2 + (buildingSize / 2.0f) + coord.y);
    }

    public static Coord GetGridCoord(Vector3 worldPosition, int buildingSize)
    {
        int x = (int)(worldPosition.x - (-GameConstants.GRID_DIMENSION_X / 2 + (buildingSize / 2.0f)));
        int y = (int)(worldPosition.z - (-GameConstants.GRID_DIMENSION_Y / 2 + (buildingSize / 2.0f)));
        return new Coord(x, y);
    }
}
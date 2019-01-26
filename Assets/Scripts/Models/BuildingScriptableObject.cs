using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "RivalClans/Building", order = 1)]
public class BuildingScriptableObject : ScriptableObject
{
    public BuildingCategory category;
    public string buildingName;
    public Resource buildCost;
    public Resource produces;
    public Coord coord;
    public float produceRateInSeconds = 10;
}
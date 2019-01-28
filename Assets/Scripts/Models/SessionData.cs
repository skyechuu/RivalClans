using System;
using System.Collections.Generic;

[Serializable]
public class SessionData
{
    public Dictionary<Coord, int> savedBuildingCoordinates;
    public Dictionary<int, BuildForSaleInfo> savedBuildingSaleInfos;
    public Dictionary<int, int> savedBuildingCounts;
    public Resource savedResources;
}


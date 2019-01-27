using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class SessionData
{
    public Dictionary<Coord, int> savedBuildingCoordinates;
    public Dictionary<int, BuildForSaleInfo> savedBuildingSaleInfos;
    public Dictionary<int, int> savedBuildingCounts;
    public Resource savedResources;


}


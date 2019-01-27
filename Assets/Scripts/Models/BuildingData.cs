
[System.Serializable]
public class BuildingData 
{
    public int id;
    public int category;
    public string buildingName;
    public Resource buildCost;
    public int produces;
    public int produceRateInSeconds;
    public int maxAmount;

    public BuildingData Clone()
    {
        return new BuildingData()
        {
            id = id,
            category = category,
            buildingName = buildingName,
            buildCost = buildCost.Clone(),
            produces = produces,
            produceRateInSeconds = produceRateInSeconds,
            maxAmount = maxAmount
        };
    }
}
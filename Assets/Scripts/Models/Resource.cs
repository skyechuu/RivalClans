

[System.Serializable]
public class Resource
{
    public int wood;
    public int rock;
    public int coin;

    public Resource(int _wood, int _rock, int _coin)
    {
        wood = _wood;
        rock = _rock;
        coin = _coin;
    }

    public void AddResource(int value, ResourceType type)
    {
        switch (type)
        {
            case ResourceType.COIN:
                AddCoin(value);
                break;
            case ResourceType.WOOD:
                AddWood(value);
                break;
            case ResourceType.ROCK:
                AddRock(value);
                break;
            default:
                break;
        }
    }

    public void SpendResource(int value, ResourceType type)
    {
        switch (type)
        {
            case ResourceType.COIN:
                RemoveCoin(value);
                break;
            case ResourceType.WOOD:
                RemoveWood(value);
                break;
            case ResourceType.ROCK:
                RemoveRock(value);
                break;
            default:
                break;
        }
    }

    public void AddWood(int _wood)
    {
        wood += _wood;
    }

    public void AddRock(int _rock)
    {
        rock += _rock;
    }

    public void AddCoin(int _coin)
    {
        coin += _coin;
    }

    public void RemoveWood(int _wood)
    {
        wood -= _wood;
    }

    public void RemoveRock(int _rock)
    {
        rock -= _rock;
    }

    public void RemoveCoin(int _coin)
    {
        coin -= _coin;
    }

}

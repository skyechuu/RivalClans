[System.Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public Coord(UnityEngine.Vector3 v)
    {
        x = (int)v.x;
        y = (int)v.z;
    }

    public UnityEngine.Vector3 GetVector3()
    {
        return new UnityEngine.Vector3(x, 0, y);
    }

    public void SetCoord(UnityEngine.Vector3 v)
    {
        x = (int)v.x;
        y = (int)v.z;
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }

    public override bool Equals(object obj)
    {
        Coord _coord = (Coord)obj;
        return _coord.x == x && _coord.y == y;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

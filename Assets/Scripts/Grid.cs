using System;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int x;
    public int y;
    internal object building;
    internal bool occupied;

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }

    internal void SetOccupied(bool v)
    {
        throw new NotImplementedException();
    }

    public void SetColor(Color c)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = c;
    }
}

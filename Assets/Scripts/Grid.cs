using UnityEngine;

public class Grid : MonoBehaviour {

    public int x;
    public int y;
    public Building building;
    public bool occupied = false;
    
    public void SetOccupied(bool value)
    {
        occupied = value;
        if (value)
            SetColor(Color.red);
        else
            SetColor(Color.white);
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }

    void SetColor(Color c)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = c;
    }
}

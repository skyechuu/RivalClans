using UnityEngine;

public class Grid : MonoBehaviour {
    
    public void SetColor(Color c)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = c;
    }

}

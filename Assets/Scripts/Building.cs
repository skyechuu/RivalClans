using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    public BuildingCategory category;
    public string buildingName;
    public Resource buildCost;
    public Resource produces;

    public Vector3 GetPinPoint()
    {
        if (category == BuildingCategory.Category2)
            return transform.position - new Vector3(0.5f, 0, 0.5f);
        return transform.position;
    }

}

﻿using UnityEngine;

public class BuildingsListView : MonoBehaviour {

    [SerializeField] Transform contentList;
    [SerializeField] GameObject buildingViewPrefab;


    void OnEnable()
    {
        if(contentList.childCount == 0)
        {
            foreach(Building b in BuildingManager.instance.availableBuildings)
            {
                GameObject go = Instantiate(buildingViewPrefab, contentList);
                go.GetComponent<BuildingButtonView>().SetBuilding(b);
                
            }
        }
    }
    
    
	void Start () {
		
	}
	
	void Update () {
		
	}
}

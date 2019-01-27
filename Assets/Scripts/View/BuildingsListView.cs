using UnityEngine;

public class BuildingsListView : MonoBehaviour {

    [SerializeField] Transform contentList;
    [SerializeField] GameObject buildingViewPrefab;


    void OnEnable()
    {
        if(contentList.childCount == 0)
        {
            foreach(var item in SessionManager.availableBuildings)
            {
                GameObject go = Instantiate(buildingViewPrefab, contentList);
                go.GetComponent<BuildingButtonView>().SetBuilding(item.Key);
            }
        }
        contentList.GetComponent<RectTransform>().sizeDelta = new Vector2(220f * (contentList.childCount + 1), 0);
    }
}

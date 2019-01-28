using UnityEngine;
using UnityEngine.Assertions;

public class BuildingsListView : MonoBehaviour {

    [SerializeField] Transform contentList;
    [SerializeField] GameObject buildingViewPrefab;

    void OnValidate()
    {
        Assert.IsNotNull(contentList, "contentList is set to null.");
        Assert.IsNotNull(buildingViewPrefab, "buildingViewPrefab is set to null.");
    }

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

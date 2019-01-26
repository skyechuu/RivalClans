using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonView : MonoBehaviour {

    public Building building;
    [SerializeField] Text title;
    [SerializeField] Text woodText;
    [SerializeField] Text rockText;
    [SerializeField] Text coinText;

    void OnEnable () {
        if(building)
            RenderTexts();
	}
	
	void Update () {
        if (gameObject.activeInHierarchy)
        {
            GetComponent<Button>().interactable = CheckForCost();
        }	
	}

    bool CheckForCost()
    {
        var currentResources = ResourcesManager.instance.GetTotalResources();
        return (building.buildCost.wood <= currentResources.wood && building.buildCost.rock <= currentResources.rock && building.buildCost.coin <= currentResources.coin);
    }

    public void OnClick()
    {
        BuildingManager.instance.Build(building);
        ResourcesManager.instance.GetTotalResources().SpendResource(building.buildCost);
        GameViewManager.instance.SetBuildMenuViewActive(false);
    }

    public void SetBuilding(Building _building)
    {
        building = _building;
        title.text = building.name;

        if (building.buildCost.wood > 0)
            woodText.text = string.Format("x{0} Wood", building.buildCost.wood);
        else
            woodText.gameObject.SetActive(false);

        if (building.buildCost.rock > 0)
            rockText.text = string.Format("x{0} Rock", building.buildCost.rock);
        else
            rockText.gameObject.SetActive(false);

        if (building.buildCost.coin > 0)
            coinText.text = string.Format("x{0} Coin", building.buildCost.coin);
        else
            coinText.gameObject.SetActive(false);

    }

    void RenderTexts()
    {
        if (building.buildCost.wood > 0)
            woodText.text = string.Format("x{0} Wood", building.buildCost.wood);
        else
            woodText.gameObject.SetActive(false);

        if (building.buildCost.rock > 0)
            rockText.text = string.Format("x{0} Rock", building.buildCost.rock);
        else
            rockText.gameObject.SetActive(false);

        if (building.buildCost.coin > 0)
            coinText.text = string.Format("x{0} Coin", building.buildCost.coin);
        else
            coinText.gameObject.SetActive(false);
    }


}

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BuildingPopupMenuView : MonoBehaviour {

    Building building;

    [SerializeField] Text nameText;
    [SerializeField] Text categoryText;
    [SerializeField] Text buildCostText;
    [SerializeField] Text productionText;
    [SerializeField] Text rateText;

    void OnValidate()
    {
        Assert.IsNotNull(nameText, "nameText is set to null.");
        Assert.IsNotNull(categoryText, "categoryText is set to null.");
        Assert.IsNotNull(buildCostText, "buildCostText is set to null.");
        Assert.IsNotNull(productionText, "productionText is set to null.");
        Assert.IsNotNull(rateText, "rateText is set to null.");
    }

    void OnEnable () {
        SetBuilding(InputManager.instance.GetSelectedBuilding());
        RenderTexts();
	}

    void OnDisable()
    {
        building = null;
    }

	void LateUpdate () {
        RenderCurrentProductionText();
	}

    public void SetBuilding(Building _building)
    {
        building = _building;
    }

    void RenderTexts()
    {
        if (!building)
            return;

        nameText.text = building.data.buildingName;
        categoryText.text = building.GetCategory().ToString();

        string costText = "Build Cost:";
        if (building.data.buildCost.wood > 0)
            costText += string.Format(" {0}w", building.data.buildCost.wood);
        if (building.data.buildCost.rock > 0)
            costText += string.Format(" {0}r", building.data.buildCost.rock);
        if (building.data.buildCost.coin > 0)
            costText += string.Format(" {0}c", building.data.buildCost.coin);
        buildCostText.text = costText;

        RenderCurrentProductionText();
        rateText.text = string.Format("every {0} second(s)", building.data.produceRateInSeconds);
    }

    private void RenderCurrentProductionText()
    {
        productionText.text = string.Format("+{0} {1}", building.data.produces, building.GetCurrentResourceType().ToString());
    }

    public void OnRemoveClick()
    {
        BuildingManager.instance.RemoveBuilding(building);
        GameViewManager.instance.SetBuildingPopupViewActive(false);
    }

    public void OnMoveClick()
    {
        building.OnMoveStarted();
        GameViewManager.instance.SetBuildingPopupViewActive(false);
        GameViewManager.instance.SetMoveConfirmButtonViewActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonView : MonoBehaviour
{
    public Building building;
    [SerializeField] Text title;
    [SerializeField] Text woodText;
    [SerializeField] Text rockText;
    [SerializeField] Text coinText;
    [SerializeField] Text remainingAmountText;

    void OnEnable()
    {
        if (building)
            RenderTexts();
    }

    void LateUpdate()
    {
        if (gameObject.activeInHierarchy)
        {
            GetComponent<Button>().interactable = CheckForCost() && CheckForRemaining();
        }
    }

    bool CheckForCost()
    {
        var currentResources = ResourcesManager.instance.GetTotalResources();
        BuildForSaleInfo info = SessionManager.availableBuildings[building];
        return (info.buildCost.wood <= currentResources.wood && info.buildCost.rock <= currentResources.rock && info.buildCost.coin <= currentResources.coin);
    }

    bool CheckForRemaining()
    {
        BuildForSaleInfo info = SessionManager.availableBuildings[building];
        return info.remaining > 0;
    }

    public void OnClick()
    {
        BuildForSaleInfo info = SessionManager.availableBuildings[building];
        ResourcesManager.instance.GetTotalResources().SpendResource(info.buildCost);
        BuildingManager.instance.Build(building);
        GameViewManager.instance.SetBuildMenuViewActive(false);
    }

    public void SetBuilding(Building _building)
    {
        building = _building;
        title.text = building.name;

        RenderTexts();
    }

    void RenderTexts()
    {
        BuildForSaleInfo info = SessionManager.availableBuildings[building];
        if (info.buildCost.wood > 0)
            woodText.text = string.Format("x{0} Wood", info.buildCost.wood);
        else
            woodText.gameObject.SetActive(false);

        if (info.buildCost.rock > 0)
            rockText.text = string.Format("x{0} Rock", info.buildCost.rock);
        else
            rockText.gameObject.SetActive(false);

        if (info.buildCost.coin > 0)
            coinText.text = string.Format("x{0} Coin", info.buildCost.coin);
        else
            coinText.gameObject.SetActive(false);

        if (info.remaining > 0)
            remainingAmountText.text = string.Format("{0} more remaining", info.remaining);
        else
            remainingAmountText.text = "You can not build anymore of this building.";
    }


}

using UnityEngine;
using UnityEngine.Assertions;

public class GameViewManager : MonoBehaviour {

    public static GameViewManager instance;

    [SerializeField] Transform buildMenuView;
    [SerializeField] Transform buildingPopupView;
    [SerializeField] Transform moveConfirmButtonView;
    
    void Awake()
    {
        Assert.IsNotNull(buildMenuView, "buildMenuView is set to null.");
        Assert.IsNotNull(buildingPopupView, "buildingPopupView is set to null.");
        Assert.IsNotNull(moveConfirmButtonView, "moveConfirmButtonView is set to null.");

        if (instance == null)
            instance = this;
    }

    public void SetBuildMenuViewActive(bool value)
    {
        buildMenuView.gameObject.SetActive(value);
    }

    public void SetBuildingPopupViewActive(bool value)
    {
        buildingPopupView.gameObject.SetActive(value);
    }

    public void SetMoveConfirmButtonViewActive(bool value)
    {
        moveConfirmButtonView.gameObject.SetActive(value);
    }
}

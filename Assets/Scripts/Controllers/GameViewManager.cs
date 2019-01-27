using UnityEngine;

public class GameViewManager : MonoBehaviour {

    public static GameViewManager instance;

    [SerializeField] Transform buildMenuView;
    [SerializeField] Transform buildingPopupView;
    [SerializeField] Transform moveConfirmButtonView;

    void Awake()
    {
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

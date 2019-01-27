using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveConfirmButtonsView : MonoBehaviour {

    Building building;

    [SerializeField] float offsetY = 2f;

    void OnEnable()
    {
        building = InputManager.instance.GetSelectedBuilding();
    }

	void LateUpdate()
    {
        transform.position = building.transform.position + (Vector3.up * offsetY);
    }

    public void OnCancel()
    {
        building.OnCancelMove();
        GameViewManager.instance.SetMoveConfirmButtonViewActive(false);
        GameViewManager.instance.SetBuildingPopupViewActive(true);
    }

    public void OnApply()
    {
        building.OnMoveEnded();
        GameViewManager.instance.SetMoveConfirmButtonViewActive(false);
        InputManager.instance.OnDeselectBuilding();
    }
}

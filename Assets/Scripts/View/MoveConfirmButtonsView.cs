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
        if(building)
            Move();
    }

    public void OnCancel()
    {
        building.OnCancelMove();
        GameViewManager.instance.SetMoveConfirmButtonViewActive(false);
        GameViewManager.instance.SetBuildingPopupViewActive(true);
    }

    public void OnApply()
    {
        var successful = building.OnMoveEnded();
        if (successful)
        {
            GameViewManager.instance.SetMoveConfirmButtonViewActive(false);
            InputManager.instance.OnDeselectBuilding();
        }
    }

    public void Move()
    {
        var viewportPoint = Camera.main.WorldToViewportPoint(building.transform.position);
        var canvas = transform.parent.GetComponent<RectTransform>();
        transform.position = new Vector3(
            viewportPoint.x * canvas.sizeDelta.x * canvas.localScale.x,
            viewportPoint.y * canvas.sizeDelta.y * canvas.localScale.y,
            viewportPoint.z * canvas.localScale.z) + offsetY * Vector3.up;
    }
}

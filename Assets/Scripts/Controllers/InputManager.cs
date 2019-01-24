using System;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    [Header("Debug")]
    [SerializeField] Grid firstGrid;
    [SerializeField] Grid onGrid;
    [SerializeField] Building selectedBuilding;

    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask gridLayer;
    
    private Vector3 buildingGrabOffset;
    private RaycastHit hit;
    [SerializeField] private bool moving = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
	
	void Update ()
    {
        if (NoInputExists)
            return;

        if (IsInputEnded)
        {
            if(moving)
            {
                if (firstGrid == onGrid)
                    CancelMoveState();
                else
                {
                    
                }
            }
            moving = false;
            buildingGrabOffset = Vector3.zero;
        }

        if (IsInputMoved)
        {
            if (OnGrid())
            {
                if (moving)
                {
                    onGrid = hit.transform.GetComponent<Grid>();
                }
            }
        }

        if (IsInputStarted)
        {
            if (OnGrid())
            {
                firstGrid = hit.transform.GetComponent<Grid>();
                if (onGrid)
                {
                    if (SameGridClicked())
                    {
                        StartMoveState();
                    }
                    else if (SameBuildingSelected())
                    {
                        onGrid = hit.transform.GetComponent<Grid>();
                        StartMoveState();
                    }
                    else
                    {
                        SelectGrid();
                    }
                }
                else
                {
                    SelectGrid();
                }
            }
        }

	}

    private void SelectGrid()
    {
        onGrid = hit.transform.GetComponent<Grid>();
        selectedBuilding = onGrid.building;
    }

    private void CancelMoveState()
    {
        throw new NotImplementedException();
    }

    private void StartMoveState()
    {
        moving = true;
        if(selectedBuilding && onGrid)
            buildingGrabOffset = selectedBuilding.transform.position - onGrid.transform.position;
    }

    bool NoInputExists
    {
#if UNITY_EDITOR
        get { return false; }
#else
        get
        {
            return Input.touchCount <= 0;
        }
#endif
    }

    Vector2 RawInputPosition
    {
#if UNITY_EDITOR
        get { return Input.mousePosition; }
#else
        get { return Input.GetTouch(0).position; }
#endif
    }

    bool IsInputStarted
    {
#if UNITY_EDITOR
        get {
            return Input.GetMouseButtonDown(0);
        }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Began; }
#endif
    }

    static bool IsInputMoved
    {
#if UNITY_EDITOR
        get { return Input.GetMouseButton(0) && (Mathf.Abs(Input.GetAxis("Mouse X")) > 0 || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0); }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Moved; }
#endif
    }
    
    static bool IsInputEnded
    {
#if UNITY_EDITOR
        get { return Input.GetMouseButtonUp(0); }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Ended; }
#endif
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(target, 0.2f);
        //Gizmos.color = Color.white;
        //Gizmos.DrawLine(inputPosition, inputPosition - buildOffset);

        /*if (onGrid)
        {
            Vector3 pinPositionOfBuilding = onGrid.transform.position - new Vector3(0.5f, 0, 0.5f);
            Gizmos.DrawWireSphere(pinPositionOfBuilding, 0.2f);
        }*/
    }

    bool OnGround()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(RawInputPosition);
        return Physics.Raycast(inputPosition, Camera.main.transform.forward, out hit, float.MaxValue, groundLayer);
    }

    bool OnGrid()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(RawInputPosition);
        return Physics.Raycast(inputPosition, Camera.main.transform.forward, out hit, float.MaxValue, gridLayer);
    }

    bool SameGridClicked()
    {
        return onGrid == hit.transform.GetComponent<Grid>();
    }

    bool SameBuildingSelected()
    {
        return selectedBuilding == hit.transform.GetComponent<Grid>().building;
    }

}

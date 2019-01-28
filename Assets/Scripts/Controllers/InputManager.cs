using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    [Header("Debug")]
    [SerializeField] Building selectedBuilding;

    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask buildingLayer;
    
    Vector3 delta;
    RaycastHit hit;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
	
	void Update ()
    {
        if (NoInputExists)
            return;

        HandleInput();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(hit.point, 0.5f);
        Gizmos.color = Color.red;
        if (selectedBuilding)
        {
            Gizmos.DrawWireSphere(hit.point - delta, 0.5f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Tools.GetGridPosition(hit.point - delta, selectedBuilding.GetSize()), 0.5f);
        }

    }

    public static bool NoInputExists
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

    public static Vector2 RawInputPosition
    {
#if UNITY_EDITOR
        get { return Input.mousePosition; }
#else
        get { return Input.GetTouch(0).position; }
#endif
    }

    public static bool IsInputStarted
    {
#if UNITY_EDITOR
        get {
            return Input.GetMouseButtonDown(0);
        }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Began; }
#endif
    }

    public static bool IsInputMoved
    {
#if UNITY_EDITOR
        get { return Input.GetMouseButton(0) && (Mathf.Abs(Input.GetAxis("Mouse X")) > 0 || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0); }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Moved; }
#endif
    }

    public static bool IsInputEnded
    {
#if UNITY_EDITOR
        get { return Input.GetMouseButtonUp(0); }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Ended; }
#endif
    }

    public static bool IsOnUI
    {

#if UNITY_EDITOR
        get { return EventSystem.current.IsPointerOverGameObject(); }
#else
        get { return EventSystem.current.IsPointerOverGameObject(0); }
#endif
    }


    public Building GetSelectedBuilding()
    {
        return selectedBuilding;
    }


    bool IsOnGround()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(RawInputPosition);
        return Physics.Raycast(inputPosition, Camera.main.transform.forward, out hit, float.MaxValue, groundLayer) && !IsOnUI;
    }

    bool IsOnBuilding()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(RawInputPosition);
        return Physics.Raycast(inputPosition, Camera.main.transform.forward, out hit, float.MaxValue, buildingLayer) && !IsOnUI;
    }
    

    Vector3 GetDelta()
    {
        return delta;
    }


    private void HandleInput()
    {
        if (IsInputEnded)
        {
            HandleInputEnded();
        }

        if (IsInputMoved)
        {
            HandleInputMoved();
        }

        if (IsInputStarted)
        {
            HandleInputStarted();
        }
    }

    private void HandleInputEnded()
    {
        delta = Vector3.zero;
        if (CameraManager.instance.state == CameraState.MOVE)
            CameraManager.instance.state = CameraState.IDLE;
    }

    private void HandleInputMoved()
    {
        if (IsOnGround())
        {
            if (CameraManager.instance.state == CameraState.MOVE)
                CameraManager.instance.MoveCamera(new Vector3(Input.GetAxisRaw("Mouse X"), 0f, Input.GetAxisRaw("Mouse Y")));
            else if (selectedBuilding)
            {
                if (selectedBuilding.GetState() == BuildingState.MOVE)
                {
                    var position = hit.point - delta;
                    selectedBuilding.OnMove(position);
                }
            }

            
        }
    }

    private void HandleInputStarted()
    {
        if (IsOnBuilding())
        {
            if (selectedBuilding)
            {
                if (IsSameBuilding())
                {
                    if (selectedBuilding.GetState() == BuildingState.MOVE)
                    {
                        delta = hit.point - selectedBuilding.transform.position;
                    }
                }
            }
            else
            {
                OnSelectBuilding(hit.transform.GetComponent<Building>());
            }
        }
        else if (IsOnGround())
        {
            CameraManager.instance.state = CameraState.MOVE;
            if (selectedBuilding && !(selectedBuilding.GetState() == BuildingState.MOVE))
            {
                OnDeselectBuilding();
            }
        }
    }

    public void OnSelectBuilding(Building building)
    {
        selectedBuilding = building;
        delta = hit.point - selectedBuilding.transform.position;
        GridManager.instance.VisualizeGridMap(selectedBuilding.GetCoord(), selectedBuilding.GetSize(), selectedBuilding.gameObject.GetInstanceID(), Color.yellow);
        if(selectedBuilding.GetState() != BuildingState.MOVE)
            GameViewManager.instance.SetBuildingPopupViewActive(true);
    }

    public void OnDeselectBuilding()
    {
        selectedBuilding = null;
        GridManager.instance.ClearGrid();
    }

    private bool IsSameBuilding()
    {
        return hit.transform.GetComponent<Building>() == selectedBuilding;
    }
}

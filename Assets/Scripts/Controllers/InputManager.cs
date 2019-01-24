using System;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    [Header("Debug")]
    [SerializeField] Building selectedBuilding;

    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask buildingLayer;

    Vector3 firstPosition;
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
        if (selectedBuilding && selectedBuilding.buildingState == BuildingState.MOVE)
        {
            selectedBuilding.OnMoveEnded();
        }
        delta = Vector3.zero;
    }

    private void HandleInputMoved()
    {
        if (IsOnGround())
        {
            if (selectedBuilding)
            {
                if (selectedBuilding.buildingState == BuildingState.MOVE)
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
            if (selectedBuilding && IsSameBuilding())
            {
                delta = hit.point - selectedBuilding.transform.position;
                selectedBuilding.OnMoveStarted();
            }
            else if (selectedBuilding && selectedBuilding.buildingState == BuildingState.MOVE)
            {
                selectedBuilding.OnCancelMove();
            }
            else
            {
                
                selectedBuilding = hit.transform.GetComponent<Building>();
                delta = hit.point - selectedBuilding.transform.position;
            }
        
        }
        else
        {
            if (selectedBuilding && selectedBuilding.buildingState == BuildingState.MOVE)
                selectedBuilding.OnCancelMove();
        }
    }
    private bool IsSameBuilding()
    {
        return hit.transform.GetComponent<Building>() == selectedBuilding;
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
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(hit.point, 0.2f);
        Gizmos.color = Color.red;
        if (selectedBuilding)
        {
            Gizmos.DrawWireSphere(hit.point - delta, 0.2f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Tools.GetGridPosition(hit.point - delta, selectedBuilding.GetSize()), 0.2f);
        }

    }

    bool IsOnGround()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(RawInputPosition);
        return Physics.Raycast(inputPosition, Camera.main.transform.forward, out hit, float.MaxValue, groundLayer);
    }

    bool IsOnBuilding()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(RawInputPosition);
        return Physics.Raycast(inputPosition, Camera.main.transform.forward, out hit, float.MaxValue, buildingLayer);
    }

    Vector3 GetDelta()
    {
        return delta;
    } 
    
    
}

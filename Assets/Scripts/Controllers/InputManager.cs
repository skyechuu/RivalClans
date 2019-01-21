using UnityEngine;

public class InputManager : MonoBehaviour {

    [SerializeField] Transform building;
    [SerializeField] LayerMask groundLayer;

    private Vector3 target;
    private Vector3 newBuildingPosition;

	void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButton(0))
        {
            var inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(inputPosition, Camera.main.transform.forward, out hit, float.MaxValue, groundLayer))
            {
                target = hit.point;
                MoveBuilding();
            }
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target, 1f);
        Gizmos.DrawRay(target, Camera.main.transform.forward*1000f);
    }

    void MoveBuilding()
    {
        float gridUnit = 1f;
        newBuildingPosition.x = Mathf.Floor(target.x / gridUnit) * gridUnit + 1f; // 1f = 2unity building / 2
        newBuildingPosition.y = 0;
        newBuildingPosition.z = Mathf.Floor(target.z / gridUnit) * gridUnit + 1f;
        building.position = newBuildingPosition;
    }
}

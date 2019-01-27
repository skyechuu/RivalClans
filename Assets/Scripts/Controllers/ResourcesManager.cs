using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager instance;

    [SerializeField] Resource totalResources;

    void Awake()
    {
        if (instance == null)
            instance = this;
        totalResources = new Resource(200, 200, 200);
    }

    public Resource GetTotalResources()
    {
        return totalResources;
    }

    public void SetTotalResources(Resource resource)
    {
        totalResources = resource.Clone();
    }

}

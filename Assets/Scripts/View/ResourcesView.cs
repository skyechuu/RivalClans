using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ResourcesView : MonoBehaviour {
    
    [SerializeField] Text wood;
    [SerializeField] Text rock;
    [SerializeField] Text coin;

    Resource resource;

    void OnValidate()
    {
        Assert.IsNotNull(wood, "wood is set to null.");
        Assert.IsNotNull(rock, "rock is set to null.");
        Assert.IsNotNull(coin, "coin is set to null.");
    }

    void LateUpdate()
    {
        resource = ResourcesManager.instance.GetTotalResources();
        wood.text = string.Format("Wood: {0}", resource.wood.ToString());
        rock.text = string.Format("Rock: {0}", resource.rock.ToString());
        coin.text = string.Format("Coin: {0}", resource.coin.ToString());
    }
}

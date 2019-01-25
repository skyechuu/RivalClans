using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesView : MonoBehaviour {


    [SerializeField] Text wood;
    [SerializeField] Text rock;
    [SerializeField] Text coin;

    Resource resource;
    
	void Update()
    {
        resource = ResourcesManager.instance.GetTotalResources();
        wood.text = string.Format("Wood: {0}", resource.wood.ToString());
        rock.text = string.Format("Rock: {0}", resource.rock.ToString());
        coin.text = string.Format("Coin: {0}", resource.coin.ToString());
    }
}

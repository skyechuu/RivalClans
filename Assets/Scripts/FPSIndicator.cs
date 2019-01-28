using UnityEngine;
using UnityEngine.UI;

public class FPSIndicator : MonoBehaviour {
    
    float deltaTime = 0.0f;
    float fps = 0.0f;

    void Update()
    {
        deltaTime += Time.deltaTime;
        deltaTime /= 2.0f;
        fps = 1.0f / deltaTime;
        GetComponent<Text>().text = fps.ToString();
    }
}

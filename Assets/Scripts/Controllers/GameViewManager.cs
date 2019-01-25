using UnityEngine;

public class GameViewManager : MonoBehaviour {

    public static GameViewManager instance;

    [SerializeField] Transform buildMenuView;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetBuildMenuViewActive(bool value)
    {
        buildMenuView.gameObject.SetActive(value);
    }
}

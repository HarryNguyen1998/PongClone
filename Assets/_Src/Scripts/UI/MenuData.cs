using UnityEngine;

public class MenuData : MonoBehaviour
{
    public GameState MenuType;

    private void Awake()
    {
        UIController.Instance.RegisterMenu(this);
    }

    private void OnDestroy()
    {
        UIController.Instance.DeregisterMenu(this);
    }
}

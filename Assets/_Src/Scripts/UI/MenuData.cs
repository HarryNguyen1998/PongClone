using UnityEngine;

public class MenuData : MonoBehaviour
{
    public GameState MenuType;

    private void Awake()
    {
        UIManager.Instance.RegisterMenu(this);
    }

    private void OnDestroy()
    {
        UIManager.Instance.DeregisterMenu(this);
    }
}

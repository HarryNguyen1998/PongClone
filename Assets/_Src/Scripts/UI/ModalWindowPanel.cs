using UnityEngine;

public class ModalWindowPanel : MonoBehaviour
{
    public GameState MenuToSwitchBackTo;

    [SerializeField] SettingsDialog _settingsDialog;

    private void Awake()
    {
        UIManager.Instance.RegisterModalWindow(this);
    }

    private void OnDestroy()
    {
        UIManager.Instance.DeregisterModalWindow();
    }

    public void TryShow()
    {
        if (_settingsDialog.IsDirty)
            gameObject.SetActive(true);
        else
            Close();
    }

    public void Close()
    {
        GameManager.Instance.ChangeState(MenuToSwitchBackTo);
        gameObject.SetActive(false);
    }
}

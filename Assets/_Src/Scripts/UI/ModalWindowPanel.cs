using UnityEngine;

public class ModalWindowPanel : MonoBehaviour
{
    public GameState MenuToSwitchBackTo;

    [SerializeField] SettingsDialog _settingsDialog;

    private void Awake()
    {
        UIController.Instance.RegisterModalWindow(this);
    }

    private void OnDestroy()
    {
        UIController.Instance.DeregisterModalWindow();
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

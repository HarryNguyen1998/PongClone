using UnityEngine;

public class GlobalController : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;

    private void OnEnable()
    {
        _inputReader.OpenSettingsMenuEvent += OpenSettingsMenu;
    }

    private void OnDisable()
    {
        _inputReader.OpenSettingsMenuEvent -= OpenSettingsMenu;
    }

    void OpenSettingsMenu()
    {
        if (!GameManager.Instance.IsInGame)
            return;

        if (GameManager.Instance.CurrentState != GameState.kSettingsMenu)
        {
            GameManager.Instance.ChangeState(GameState.kSettingsMenu);
        }
        else
        {
            UIController.Instance.ShowApplyOrNo(GameState.kGameplay);
        }
    }
}

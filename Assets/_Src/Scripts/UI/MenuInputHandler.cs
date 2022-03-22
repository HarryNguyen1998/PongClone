using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuInputHandler : MonoBehaviour
{
    [SerializeField] GameState _menuToSwitchTo;
    [SerializeField] SettingsDialog _settingsDialog;

    void Start()
    {
#if UNITY_WEBGL
        if (_menuToSwitchTo == GameState.kQuit)
            gameObject.SetActive(false);
#endif
    }

    public void ChangeMenu()
    {
        if (_menuToSwitchTo == GameState.kSettings)
            GameStateEventRelayer.Instance.ChangeState(_menuToSwitchTo, true);
        else
            GameStateEventRelayer.Instance.ChangeState(_menuToSwitchTo);
    }

    public void ShouldShowPrompt()
    {
        _settingsDialog.TryShowPrompt(_menuToSwitchTo);
    }
}

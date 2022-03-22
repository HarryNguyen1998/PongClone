using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

// Handles registration of input to Player
public class InputHandler : MonoBehaviour
{
    // References
    [SerializeField] PlayerController _player1;
    [SerializeField] PlayerController _player2;
    [SerializeField] SettingsDialog _settingsDialog;

    // Class members
    GameInput _inputAsset;

    void OnEnable()
    {
        if (_inputAsset == null)
            _inputAsset = new GameInput();

        _inputAsset.UI.Enable();
        _inputAsset.UI.OpenSettingsMenu.performed += OpenSettingsMenu;

        _inputAsset.Player.Enable();
        _inputAsset.Player.Player1Move.performed += _player1.Move;
        _inputAsset.Player.Player1Move.canceled += _player1.Move;
        _inputAsset.Player.Player2Move.performed += _player2.Move;
        _inputAsset.Player.Player2Move.canceled += _player2.Move;

        GameStateEventHandler.Instance.GameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        _inputAsset.UI.Disable();
        _inputAsset.UI.OpenSettingsMenu.performed -= OpenSettingsMenu;

        _inputAsset.Player.Disable();
        _inputAsset.Player.Player1Move.performed -= _player1.Move;
        _inputAsset.Player.Player1Move.canceled -= _player1.Move;
        _inputAsset.Player.Player2Move.performed -= _player2.Move;
        _inputAsset.Player.Player2Move.canceled -= _player2.Move;

        GameStateEventHandler.Instance.GameStateChanged -= OnGameStateChanged;
    }

    void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.kGameOver)
        {
            _player1.GetComponent<ControllerSwitcher>().enabled = false;
            _player2.GetComponent<ControllerSwitcher>().enabled = false;
            _inputAsset.Disable();
        }
        // Don't need else because the scene is reloaded -> Everything is reset
    }

    void OpenSettingsMenu(InputAction.CallbackContext ctx)
    {
        if (GameStateEventRelayer.Instance.PeekState() == GameState.kSettings)
            _settingsDialog.TryShowPrompt(GameState.kNone);
        else
            GameStateEventRelayer.Instance.ChangeState(GameState.kSettings, true);
    }
}

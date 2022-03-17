using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


using UnityEngine.EventSystems;

[CreateAssetMenu]
public class InputReader : ScriptableObject, GameInput.IGlobalActions, GameInput.IPlayerActions
{
    public event Action OpenSettingsMenuEvent;
    public event Action<Vector2> MoveEvent;

    public bool IsLeftPad;

    GameInput _inputAsset;

    private void OnEnable()
    {
        //GameManager.Instance.RoundWasOver += DisableSettingsInput;

        if (_inputAsset == null)
        {
            _inputAsset = new GameInput();
            _inputAsset.Global.SetCallbacks(this);
            _inputAsset.Player.SetCallbacks(this);
            if (IsLeftPad)
                _inputAsset.bindingMask = new InputBinding { groups = "KeyboardLeft&GamepadLeft" };
            else
                _inputAsset.bindingMask = new InputBinding { groups = "KeyboardRight&GamepadRight" };
        }
        _inputAsset.Global.Enable();
        EnableGameplayInput();
    }

    private void DisableSettingsInput(bool leftWon)
    {
        //StartCoroutine(WaitTillRoundRestart)
    }

    IEnumerator WaitTillRoundRestart()
    {
        yield return new WaitForSeconds(.1f);
    }


    private void OnDisable()
    {
        //GameManager.Instance.RoundWasOver -= DisableSettingsInput;
        DisableGameplayInput();
        _inputAsset.Global.Disable();
    }

    public void OnOpenSettingsMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OpenSettingsMenuEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void EnableGameplayInput()
    {
        _inputAsset.Player.Enable();
    }

    public void DisableGameplayInput()
    {
        _inputAsset.Player.Disable();
    }

    [SerializeField] GameInput _gameInput;
    [SerializeField] InputActionReference _actionToRemap;

    InputActionRebindingExtensions.RebindingOperation _rebindingOp;
    void Rebind()
    {
        // OnClick, the GameObject stays selected, which in this case the rebind operation is
        // triggered again. This prevents that.
        EventSystem.current.SetSelectedGameObject(null);

        // Rebind is only possible after action is disabled
        _actionToRemap.action.Disable();

        // button.text = "Press any button"

        _rebindingOp = _actionToRemap.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
                {
                    //button.text = InputControlPath.ToHumanReadableString(_actionToRemap.action.bindings[0].effectivePath,
                    //    InputControlPath.HumanReadableStringOptions.OmitDevice);

                    _rebindingOp.Dispose();

                    _actionToRemap.action.Enable();
                }).Start();
    }

}




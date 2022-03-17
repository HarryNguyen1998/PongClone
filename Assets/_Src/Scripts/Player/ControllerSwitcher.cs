using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PadData))]
public class ControllerSwitcher : MonoBehaviour
{
    // Refererences
    [SerializeField] PlayerController _playerController;
    [SerializeField] AIController _aiController;

    PadData _padData;

    // Class members
    Vector3 _originalPos;

    void Awake()
    {
        _padData = GetComponent<PadData>();
    }

    void OnEnable()
    {
        SettingsDialog.GameSettingsChanged += SetAIOrPlayer;
        GameManager.Instance.GameStateChanged += DisableMovement;
    }

    void OnDisable()
    {
        GameManager.Instance.GameStateChanged -= DisableMovement;
        SettingsDialog.GameSettingsChanged -= SetAIOrPlayer;
        _playerController.enabled = false;
        _aiController.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    void Start()
    {
        _originalPos = GetComponent<Transform>().position;
        SetAIOrPlayer();
    }

    void SetAIOrPlayer()
    {
        if (_padData.IsLeftPad)
        {
            if (GameManager.Instance.CurrentSettings.IsLeftPadAIControlled)
            {
                UseAIController();
            }
            else
            {
                UsePlayerController();
            }
        }
        else
        {
            if (GameManager.Instance.CurrentSettings.IsRightPadAIControlled)
            {
                UseAIController();
            }
            else
            {
                UsePlayerController();
            }
        }
    }

    void UsePlayerController()
    {
        _playerController.enabled = true;
        _aiController.enabled = false;
    }
    
    void UseAIController()
    {
        _playerController.enabled = false;
        _aiController.enabled = true;
    }

    void DisableMovement(GameState newState)
    {
        if (newState == GameState.kGameOverMenu)
            enabled = false;
    }
    
}

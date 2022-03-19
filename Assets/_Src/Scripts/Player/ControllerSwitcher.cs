using UnityEngine;

// Whether the pad is controlled by AI or player
[RequireComponent(typeof(PadData), typeof(PlayerController), typeof(AIController))]
public class ControllerSwitcher : MonoBehaviour
{
    // Refererences
    PadData _padData;
    PlayerController _playerController;
    AIController _aiController;

    void Awake()
    {
        _padData = GetComponent<PadData>();
        _playerController = GetComponent<PlayerController>();
        _aiController = GetComponent<AIController>();
    }

    void OnEnable()
    {
        SettingsDialog.GameSettingsChanged += SetAIOrPlayer;
    }

    void OnDisable()
    {
        SettingsDialog.GameSettingsChanged -= SetAIOrPlayer;
        _playerController.enabled = false;
        _aiController.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    void Start()
    {
        SetAIOrPlayer();
    }

    void SetAIOrPlayer()
    {
        if (_padData.IsLeftPad)
        {
            if (GameManager.Instance.CurrentSettings.IsLeftPadAIControlled)
                UseAIController();
            else
                UsePlayerController();
        }
        else
        {
            if (GameManager.Instance.CurrentSettings.IsRightPadAIControlled)
                UseAIController();
            else
                UsePlayerController();
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

}

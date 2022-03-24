using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class SettingsDialog : MonoBehaviour
{
    public static event Action GameSettingsChanged;

    [SerializeField] ModalWindowPanel _prompt;

    [Header("Contents")]
    // References
    [SerializeField] Toggle _leftPadAIToggle;
    [SerializeField] Toggle _rightPadAIToggle;
    [SerializeField] Slider _roundCntSlider;
    [SerializeField] Slider _padSpdSlider;
    [SerializeField] Slider _ballSpdSlider;
    [SerializeField] Button _backBtn;
    [SerializeField] Button _backToMainMenuBtn;

    // Members
    bool _isDirty; 

    Resolution[] _resolutions;
    GameplaySettingsPOD _tempChanges;

    void Awake()
    {
        if (GameStateEventRelayer.Instance.IsInGame())
        {
            _roundCntSlider.interactable = false;
            _backBtn.gameObject.SetActive(true);
        }
        else
        {
            _roundCntSlider.interactable = true;
            _backBtn.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        _tempChanges = GameStateEventRelayer.Instance.CurrentSettings;
        UpdateSettingsToUI();
    }

    void Start()
    {
        _isDirty = false;
    }

    public void TryShowPrompt(GameState newState = GameState.kNone)
    {
        if (_isDirty)
            _prompt.Show();
        else
            GameStateEventRelayer.Instance.PopState(newState);
    }

    public void SetLeftPad(bool isAIControlled)
    {
        _tempChanges.IsLeftPadAIControlled = isAIControlled;
        _isDirty = true;
    }

    public void SetRightPad(bool isAIControlled)
    {
        _tempChanges.IsRightPadAIControlled = isAIControlled;
        _isDirty = true;
    }

    public void SetRoundCnt(float value)
    {
        _tempChanges.RoundCnt = (int)value;
        _isDirty = true;
    }

    public void SetPadSpd(float value)
    {
        _tempChanges.PadSpd = value;
        _isDirty = true;
    }

    public void SetBallSpd(float value)
    {
        _tempChanges.BallSpd = value;
        _isDirty = true;
    }

    public void ApplySettings()
    {
        GameStateEventRelayer.Instance.CurrentSettings = _tempChanges;
        _isDirty = false;
        GameSettingsChanged?.Invoke();
    }

    public void RevertSettings()
    {
        _tempChanges = GameStateEventRelayer.Instance.CurrentSettings;
        UpdateSettingsToUI();
        _isDirty = false;
    }

    public void ResetSettings()
    {
        GameStateEventRelayer.Instance.ResetSettings();
        UpdateSettingsToUI();
    }

    public void UpdateSettingsToUI()
    {
        _leftPadAIToggle.isOn = GameStateEventRelayer.Instance.CurrentSettings.IsLeftPadAIControlled;
        _rightPadAIToggle.isOn = GameStateEventRelayer.Instance.CurrentSettings.IsRightPadAIControlled;
        _roundCntSlider.value = GameStateEventRelayer.Instance.CurrentSettings.RoundCnt;
        _padSpdSlider.value = GameStateEventRelayer.Instance.CurrentSettings.PadSpd;
        _ballSpdSlider.value = GameStateEventRelayer.Instance.CurrentSettings.BallSpd;
        _isDirty = false;
    }

}

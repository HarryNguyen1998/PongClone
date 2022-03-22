using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils;

public class SettingsDialog : MonoBehaviour
{
    public static event Action GameSettingsChanged;

    public static event Action<float> ResolutionChanged;

    [SerializeField] ModalWindowPanel _prompt;

    [Header("Contents")]
    // References
    [SerializeField] TMP_Dropdown _resDropdown;
    [SerializeField] Toggle _fullScreenToggle;
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
        // Populate resolutions
        int refreshRate = Screen.currentResolution.refreshRate;
        _resolutions = new Resolution[] {
            new Resolution() {width = 800, height = 600, refreshRate = refreshRate},
            new Resolution() {width = 1024, height = 768, refreshRate = refreshRate},
            new Resolution() {width = 1280, height = 720, refreshRate = refreshRate},
            new Resolution() {width = 1280, height = 960, refreshRate = refreshRate},
            new Resolution() {width = 1920, height = 1080, refreshRate = refreshRate},
        };
        _resDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < _resolutions.Length; ++i)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
        }
        _resDropdown.AddOptions(options);

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
        // Just hide screen settings since it doesn't work well in WebGL build
#if UNITY_WEBGL
        _resDropdown.transform.parent.gameObject.SetActive(false);
#endif
        _tempChanges = GameStateEventRelayer.Instance.CurrentSettings;
        UpdateSettingsToUI();
    }

    void Start()
    {
        // Initialize values
        _resDropdown.value = _tempChanges.ResolutionIndex;
        SetResolution(_tempChanges.ResolutionIndex);
        SetFullScreen(_tempChanges.IsFullScreen);
        _resDropdown.RefreshShownValue();
        _isDirty = false;
    }

    public void TryShowPrompt(GameState newState)
    {
        if (_isDirty)
            _prompt.Show();
        else
            GameStateEventRelayer.Instance.ChangeState(newState);
    }

    public void SetResolution(int index)
    {
#if !UNITY_WEBGL
        _tempChanges.ResolutionIndex = index;
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);
        _tempChanges.AspectRatio = (float)Screen.width / Screen.height;
        
        ResolutionChanged?.Invoke(_tempChanges.AspectRatio);
        _isDirty = true;
#endif
    }

    public void SetFullScreen(bool isFullScreen)
    {
#if !UNITY_WEBGL
        Screen.fullScreen = isFullScreen;
        _tempChanges.IsFullScreen = isFullScreen;
        _isDirty = true;
#endif
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
        _resDropdown.value = GameStateEventRelayer.Instance.CurrentSettings.ResolutionIndex;
        _resDropdown.RefreshShownValue();
        _fullScreenToggle.isOn = GameStateEventRelayer.Instance.CurrentSettings.IsFullScreen;
        _leftPadAIToggle.isOn = GameStateEventRelayer.Instance.CurrentSettings.IsLeftPadAIControlled;
        _rightPadAIToggle.isOn = GameStateEventRelayer.Instance.CurrentSettings.IsRightPadAIControlled;
        _roundCntSlider.value = GameStateEventRelayer.Instance.CurrentSettings.RoundCnt;
        _padSpdSlider.value = GameStateEventRelayer.Instance.CurrentSettings.PadSpd;
        _ballSpdSlider.value = GameStateEventRelayer.Instance.CurrentSettings.BallSpd;
        _isDirty = false;
    }

}

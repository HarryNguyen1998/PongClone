using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsDialog : MonoBehaviour
{
    public static event Action GameSettingsChanged;

    public static event Action<float> ResolutionChanged;

    [Header("Contents")]
    // References
    [SerializeField] TMP_Dropdown _resDropdown;
    [SerializeField] Toggle _fullScreenToggle;
    [SerializeField] Toggle _leftPadAIToggle;
    [SerializeField] Toggle _rightPadAIToggle;
    [SerializeField] Slider _roundCntSlider;
    [SerializeField] Slider _padSpdSlider;
    [SerializeField] Slider _ballSpdSlider;
    [SerializeField] Button _backToMainMenuBtn;

    // Members
    public bool IsDirty { get; private set; }

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

    }

    void OnEnable()
    {
        // Just hide screen settings since it doesn't work well in WebGL build
#if UNITY_WEBGL
        _resDropdown.transform.parent.gameObject.SetActive(false);
#endif
        if (GameManager.Instance.IsInGame)
        {
            _roundCntSlider.interactable = false;
        }
        else
        {
            _roundCntSlider.interactable = true;
        }

        _tempChanges = GameManager.Instance.CurrentSettings;
        UpdateSettingsToUI();
    }

    void Start()
    {
        // Initialize values
        _resDropdown.value = _tempChanges.ResolutionIndex;
        SetResolution(_tempChanges.ResolutionIndex);
        SetFullScreen(_tempChanges.IsFullScreen);
        _resDropdown.RefreshShownValue();
        IsDirty = false;
    }

    public void SetResolution(int index)
    {
#if !UNITY_WEBGL
        _tempChanges.ResolutionIndex = index;
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);
        _tempChanges.AspectRatio = (float)Screen.width / Screen.height;
        
        ResolutionChanged?.Invoke(_tempChanges.AspectRatio);
        IsDirty = true;
#endif
    }

    public void SetFullScreen(bool isFullScreen)
    {
#if !UNITY_WEBGL
        Screen.fullScreen = isFullScreen;
        _tempChanges.IsFullScreen = isFullScreen;
        IsDirty = true;
#endif
    }

    public void SetLeftPad(bool isAIControlled)
    {
        _tempChanges.IsLeftPadAIControlled = isAIControlled;
        IsDirty = true;
    }

    public void SetRightPad(bool isAIControlled)
    {
        _tempChanges.IsRightPadAIControlled = isAIControlled;
        IsDirty = true;
    }

    public void SetRoundCnt(float value)
    {
        _tempChanges.RoundCnt = (int)value;
        IsDirty = true;
    }

    public void SetPadSpd(float value)
    {
        _tempChanges.PadSpd = value;
        IsDirty = true;
    }

    public void SetBallSpd(float value)
    {
        _tempChanges.BallSpd = value;
        IsDirty = true;
    }

    public void ApplySettings()
    {
        GameManager.Instance.CurrentSettings = _tempChanges;
        GameSettingsChanged?.Invoke();
    }

    public void ResetSettings()
    {
        GameManager.Instance.ResetSettings();
        UpdateSettingsToUI();
    }

    public void UpdateSettingsToUI()
    {
        _resDropdown.value = GameManager.Instance.CurrentSettings.ResolutionIndex;
        _resDropdown.RefreshShownValue();
        _fullScreenToggle.isOn = GameManager.Instance.CurrentSettings.IsFullScreen;
        _leftPadAIToggle.isOn = GameManager.Instance.CurrentSettings.IsLeftPadAIControlled;
        _rightPadAIToggle.isOn = GameManager.Instance.CurrentSettings.IsRightPadAIControlled;
        _roundCntSlider.value = GameManager.Instance.CurrentSettings.RoundCnt;
        _padSpdSlider.value = GameManager.Instance.CurrentSettings.PadSpd;
        _ballSpdSlider.value = GameManager.Instance.CurrentSettings.BallSpd;
        IsDirty = false;
    }

}

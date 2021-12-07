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

    GameManager _gameManager;
    Resolution[] _resolutions;
    GameplaySettingsPOD _tempChanges;

    void Awake()
    {
        _gameManager = GameManager.Instance;

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
        if (_gameManager.IsInGame)
        {
            _roundCntSlider.interactable = false;
        }
        else
        {
            _roundCntSlider.interactable = true;
        }

        _tempChanges = _gameManager.CurrentSettings.DeepCopy();
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
        _tempChanges.ResolutionIndex = index;
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);
        _tempChanges.AspectRatio = (float)Screen.width / Screen.height;
        
        ResolutionChanged?.Invoke(_tempChanges.AspectRatio);
        IsDirty = true;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        _tempChanges.IsFullScreen = isFullScreen;
        IsDirty = true;
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
        _gameManager.CurrentSettings = _tempChanges;
        _gameManager.SaveToFile();
        _tempChanges = null;
        GameSettingsChanged?.Invoke();
    }

    public void ResetSettings()
    {
        _gameManager.ResetSettings();
        UpdateSettingsToUI();
    }

    public void UpdateSettingsToUI()
    {
        _resDropdown.value = _gameManager.CurrentSettings.ResolutionIndex;
        _resDropdown.RefreshShownValue();
        _fullScreenToggle.isOn = _gameManager.CurrentSettings.IsFullScreen;
        _leftPadAIToggle.isOn = _gameManager.CurrentSettings.IsLeftPadAIControlled;
        _rightPadAIToggle.isOn = _gameManager.CurrentSettings.IsRightPadAIControlled;
        _roundCntSlider.value = _gameManager.CurrentSettings.RoundCnt;
        _padSpdSlider.value = _gameManager.CurrentSettings.PadSpd;
        _ballSpdSlider.value = _gameManager.CurrentSettings.BallSpd;
        IsDirty = false;
    }

}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum GameState
{
    kMainMenu,
    kGameplay,
    kSettingsMenu,
    kGameOverMenu,
    kQuit,
}

public sealed class GameManager : MonoBehaviour
{
    // References
    [SerializeField] GameSettings _gameSettingsSO;

    // Members
    public event Action<GameState> GameStateChanged;
    public event Action<bool> RoundWasOver;

    public GameplaySettingsPOD CurrentSettings { get; set; }
    public int ScoreLeft { get; private set; }
    public int ScoreRight { get; private set; }
    public bool LeftWon { get; private set; }
    public bool IsInGame { get; set; }
    public GameState CurrentState { get; private set; }

    bool _isPaused;

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        _isPaused = false;
    }

    void Start()
    {
        ResetSettings();
    }

    public void ResetSettings()
    {
        CurrentSettings = _gameSettingsSO.DefaultSettings;
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        if (newState == GameState.kSettingsMenu)
        {
            Pause();
        }
        else
        {
            if (_isPaused)
                Resume();
        }

        switch (newState)
        {
            case GameState.kMainMenu:
            {
                DOTween.KillAll();
                SceneManager.LoadSceneAsync("PongClone");
                if (IsInGame)
                {
                    IsInGame = false;
                }

                break;
            }
            case GameState.kGameplay:
            {
                // Transition between gameplay and settings won't reload the Scene.
#if UNITY_WEBGL
                CurrentSettings.IsFullScreen = Screen.fullScreen;
#endif
                if (!IsInGame)
                {
                    ScoreLeft = 0;
                    ScoreRight = 0;
                    DOTween.KillAll();
                    SceneManager.LoadSceneAsync("PongClone");
                }
                IsInGame = true;
                break;
            }
            case GameState.kGameOverMenu:
            {
                IsInGame = false;
                break;
            }
            case GameState.kQuit:
            {
                Quit();
                break;
            }
        }

        GameStateChanged?.Invoke(CurrentState);
    }

    public void IncrementScore(bool leftWon)
    {
        LeftWon = leftWon;

        if (leftWon)
            ++ScoreLeft;
        else
            ++ScoreRight;

        // We don't want a gameover in main menu
        if (CurrentState == GameState.kGameplay &&
            (ScoreLeft >= CurrentSettings.RoundCnt || ScoreRight >= CurrentSettings.RoundCnt))
            ChangeState(GameState.kGameOverMenu);

        RoundWasOver?.Invoke(leftWon);
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0.0f;
    }

    public void Quit()
    {
        if (Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }

}
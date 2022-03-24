using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Utils;

public class GameStateEventHandler : MonoBehaviour
{
    // Members
    public event Action<GameState> GameStateChanged;
    bool _isPaused;

    public static GameStateEventHandler Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActuallyChangeState()
    {
        // @note Why put here but not in GameStateEventRelayer? Because when game 1st loads,
        // animator goes into MainMenu state, which calls this func. This is put here to prevent the
        // map from being reloaded
        if (GameStateEventRelayer.Instance.PrevState == GameStateEventRelayer.Instance.PeekState())
            return;

        if (GameStateEventRelayer.Instance.PeekState() == GameState.kSettings)
        {
            _isPaused = true;
            Time.timeScale = 0.0f;
        }
        else
        {
            if (_isPaused)
            {
                _isPaused = false;
                Time.timeScale = 1.0f;
            }
        }


        switch (GameStateEventRelayer.Instance.PeekState())
        {
            case GameState.kMainMenu:
            case GameState.kGameplay:
            {
                if (GameStateEventRelayer.Instance.PrevState != GameState.kSettings)
                {
                    GameStateEventRelayer.Instance.ResetScore();
                    DOTween.KillAll();
                    SceneManager.LoadSceneAsync("PongClone");
                }
                break;
            }
            case GameState.kSettings:
                break;
            case GameState.kGameOver:
                break;
            case GameState.kQuit:
            {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                break;
            }
        }

        Instance.GameStateChanged?.Invoke(GameStateEventRelayer.Instance.PeekState());
    }

}
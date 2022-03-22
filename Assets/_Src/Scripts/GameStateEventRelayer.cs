using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;

public enum GameState
{
    kNone,
    kMainMenu,
    kGameplay,
    kSettings,
    kGameOver,
    kQuit,
}

public class GameStateEventRelayer : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] GameSettings _gameSettingsSO;

    public GameplaySettingsPOD CurrentSettings { get; set; }

    List<GameState> _stateStack = new List<GameState>();
    public GameState PrevState { get; private set; }
    // Score handling
    public event Action<bool> RoundWasOver;
    public int ScoreLeft { get; private set; }
    public int ScoreRight { get; private set; }
    public bool LeftWon { get; private set; }


    public static GameStateEventRelayer Instance { get; private set; }
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        _stateStack.Add(GameState.kMainMenu);

        // @note Why a None state? Prevent edge case: PrevState == kMainMenu, MainMenu -> Settings
        // -> MainMenu, current state would be MainMenu == PrevState, cond is true and
        // GameStateEventHandler.ActuallyChangeState() won't change the state
        PrevState = GameState.kNone;
    }

    void Start()
    {
        ResetSettings();
    }

    public GameState PeekState()
    {
        return _stateStack[_stateStack.Count - 1];
    }

    public void ChangeState(GameState newState = GameState.kNone, bool shouldAppend = false)
    {
        PrevState = PeekState();
        if (newState == GameState.kNone)
        {
            // Settings -> MainMenu through keyboard btn
            Assert.IsTrue(_stateStack.Count == 2, "State stack should never be empty.");
            _stateStack.RemoveAt(_stateStack.Count - 1);

        }
        else
        {
            // MainMenu -> Settings
            if (shouldAppend)
            {
                Assert.IsTrue(_stateStack.Count == 1, "State stack should only have 1 or 2 states at all times.");
                _stateStack.Add(newState);
            }
            // MainMenu -> Gameplay, or currently in Gameplay -> Settings, want to go to MainMenu
            else
            {
                // If MainMenu -> Gameplay then nothing, else it's SomeState + Settings, so
                // SomeState is last state
                if (_stateStack.Count == 2)
                    PrevState = _stateStack[0];
                _stateStack.Clear();
                _stateStack.Add(newState);
            }
        }

        InformFSM();
    }

    public bool IsInGame()
    {
        return (_stateStack[_stateStack.Count - 1] == GameState.kGameplay ||
            (_stateStack.Count == 2 && _stateStack[0] == GameState.kGameplay));
    }

    public void ResetSettings()
    {
        CurrentSettings = _gameSettingsSO.DefaultSettings;
    }

    public void ResetScore()
    {
        ScoreLeft = 0;
        ScoreRight = 0;
    }

    public void IncrementScore(bool leftWon)
    {
        LeftWon = leftWon;

        if (leftWon)
            ++ScoreLeft;
        else
            ++ScoreRight;

        // We don't want a gameover in main menu
        if (IsInGame() && (ScoreLeft >= CurrentSettings.RoundCnt || ScoreRight >= CurrentSettings.RoundCnt))
            ChangeState(GameState.kGameOver);

        RoundWasOver?.Invoke(leftWon);
    }

    void InformFSM()
    {
        switch (_stateStack[_stateStack.Count - 1])
        {
            case GameState.kMainMenu:
                _animator.SetTrigger("MainMenu");
                break;
            case GameState.kGameplay:
                _animator.SetTrigger("Gameplay");
                break;
            case GameState.kSettings:
                _animator.SetTrigger("Settings");
                break;
            case GameState.kGameOver:
                _animator.SetTrigger("GameOver");
                break;
            case GameState.kQuit:
                _animator.SetTrigger("Quit");
                break;
        }
    }
}

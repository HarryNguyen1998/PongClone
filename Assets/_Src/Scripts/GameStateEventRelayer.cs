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

    // Only use this for 1 state transition. If you have 2 state, use PopState instead
    public void ChangeState(GameState newState = GameState.kNone, bool shouldAppend = false)
    {
        if (newState == GameState.kNone)
        {
            // This is case 1 of PopState
        }
        else
        {
            Assert.IsTrue(_stateStack.Count == 1, "State stack should only have 1 or 2 states at all times.");

            PrevState = PeekState();
            // 1 state transition to Settings, e.g., MainMenu -> Settings
            if (shouldAppend)
                _stateStack.Add(newState);
            // 1 state transition, e.g., MainMenu -> Gameplay
            else
            {
                _stateStack.RemoveAt(_stateStack.Count - 1);
                _stateStack.Add(newState);
            }
        }

        InformFSM();
    }

    // Case 1: Gameplay+Settings, pop back to Gameplay, then pass in stateToReplace=kNone (keyboard)
    // or stateToReplace = PeekState() (menu)
    // Case 2: Gameplay+Settings, you want to go to MainMenu, then pass stateToReplace=MainMenu,
    // PopState() pops Settings, and ChangeState will replace the last state to MainMenu, as intended
    public void PopState(GameState stateToReplace = GameState.kNone)
    {
        Assert.IsTrue(_stateStack.Count == 2, "State stack should never be empty.");
        PrevState = PeekState();
        _stateStack.RemoveAt(_stateStack.Count - 1);

        // Case 1
        if (stateToReplace == GameState.kNone ||
            stateToReplace == PeekState())
            ChangeState(GameState.kNone);
        // Case 2
        else
            ChangeState(stateToReplace);
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
        this.Log(msg: $"Prev {PrevState}, Current {PeekState()}");
        switch (_stateStack[_stateStack.Count - 1])
        {
            case GameState.kMainMenu:
                _animator.SetTrigger(Animator.StringToHash("MainMenu"));
                break;
            case GameState.kGameplay:
                _animator.SetTrigger(Animator.StringToHash("Gameplay"));
                break;
            case GameState.kSettings:
                _animator.SetTrigger(Animator.StringToHash("Settings"));
                break;
            case GameState.kGameOver:
                _animator.SetTrigger(Animator.StringToHash("GameOver"));
                break;
            case GameState.kQuit:
                _animator.SetTrigger(Animator.StringToHash("Quit"));
                break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    [SerializeField] private UiManager uiManager;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Spawner spawner;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private SoundManager soundManager;

    [Header("Gamestate")]
    public GameState state;
    public enum GameState { MainMenu, Gameplay, Pause, Options, GameEnd, Upgrade };
    private GameState currentState;
    private GameState beforeOptions;

    [Header("Game Values")]
    public int winningLevel;
    internal bool isPlaying;
    private bool isNewRun = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);

        SetState(GameState.MainMenu);
    }


    private void OnEnable()
    {
        Actions.OnGameOver += PlayerDied;
        Actions.OnGameWin += GameWin;
    }

    private void OnDisable()
    {
        Actions.OnGameOver -= PlayerDied;
        Actions.OnGameWin -= GameWin;
    }

    public void LoadState(string stateName)
    {
        if (Enum.TryParse(stateName, out GameState gamestate))
            LoadState(gamestate);
        else if (stateName == "beforeOptions")
            LoadState(beforeOptions);
        else
            Debug.LogError(stateName + " doesn't exist");
    }

    private void LoadState(GameState newState)
    {
        if (newState == GameState.Options)
            beforeOptions = currentState;  // Store the current state before entering Options

        SetState(newState);
    }


    private void SetState(GameState _state)
    {
        state = _state;
        currentState = state;

        switch (state)
        {
            case GameState.MainMenu: MainMenu(); break;
            case GameState.Gameplay: Gameplay(); break;
            case GameState.Options: Options(); break;
            case GameState.Pause: Pause(); break;
            case GameState.GameEnd: GameWin(); break;
            case GameState.Upgrade: Upgrades(); break;
        }
    }

    public void EscapeState()
    {
        switch (state)
        {
            case GameState.Pause: SetState(GameState.Gameplay); break;
            case GameState.Gameplay: SetState(GameState.Pause); break;
        }
    }

    private void MainMenu()
    {
        isPlaying = false;
        isNewRun = true;
        Time.timeScale = 1;
        soundManager.PlayMenu();
        uiManager.MainMenu_UI();
    }

    private void Gameplay()
    {
        if(isNewRun)
            Actions.OnGameplay();

        isNewRun = false;
        isPlaying = true;
        Time.timeScale = 1;
        uiManager.Gameplay_UI();
    }
    private void Upgrades()
    {
        isPlaying = false;
        Time.timeScale = 0;
        upgradeManager.UpdateAllButtons();
        uiManager.Results_UI();
    }


    private void Pause()
    {
        Time.timeScale = 0;
        isPlaying = false;
        uiManager.Pause_UI();
    }

    private void Options()
    {
        uiManager.Options_UI();
    }

    private void GameWin()
    {
        isNewRun = true;
        uiManager.GameOver_UI();
    }

    private void PlayerDied()
    {
        isNewRun = true;
        LoadState(GameState.Upgrade);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}

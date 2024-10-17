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
    public bool isEndless = false;
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
        Actions.OnGameOver += Upgrades;
        Actions.OnGameWin += GameWin;
    }

    private void OnDisable()
    {
        Actions.OnGameOver -= Upgrades;
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
        PlayingState(false, true);
        soundManager.PlayMenu();
        uiManager.MainMenu_UI();
    }

    private void Gameplay()
    {
        Debug.Log(isNewRun);
        if(isNewRun)
        {
            Actions.OnGameplay();

        }

        PlayingState(true, false);
        uiManager.Gameplay_UI();
    }
    private void Upgrades()
    {
        PlayingState(false, true);
        upgradeManager.UpdateAllButtons();
        uiManager.Results_UI();
    }


    private void Pause()
    {
        PlayingState(false, false);
        uiManager.Pause_UI();
    }

    private void Options()
    {
        uiManager.Options_UI();
    }

    private void GameWin()
    {
        PlayingState(false, true);
        uiManager.GameOver_UI();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    private void PlayingState(bool currentlyPlaying, bool newRun)
    {
        isPlaying = currentlyPlaying;
        isNewRun = newRun;

        if (isPlaying)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void IsEndless(bool endless)
    {
        isEndless = endless;
    }
}

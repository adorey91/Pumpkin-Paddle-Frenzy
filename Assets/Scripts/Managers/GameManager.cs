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
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private SoundManager soundManager;

    [Header("Gamestate")]
    public GameState state;
    public enum GameState { MainMenu, Gameplay, Upgrades, Pause, Options, GameEnd };
    private GameState currentState;
    private GameState beforeOptions;

    [Header("Game Values")]
    public float moveSpeed; // used to move the scrolling level
    public int winningLevel;
    internal bool isPlaying;


    //[SerializeField] private PlayerController player;
    internal UnityEvent gamePaused = new UnityEvent();
    internal UnityEvent onPlay = new UnityEvent();
    internal UnityEvent onGameOver = new UnityEvent();
    internal UnityEvent onPlayerWin = new UnityEvent();

    // player


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
            case GameState.Upgrades: PlayerDied(); break;
            case GameState.Options: Options(); break;
            case GameState.Pause: Pause(); break;
            case GameState.GameEnd: GameWin(); break;
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
        gamePaused.Invoke();
        moveSpeed = 0;
        //healthSystem.UpdateHealthStats();
        //soundManager.PlayAudio("MainMenu");
        uiManager.MainMenu_UI();
    }

    private void Gameplay()
    {
        onPlay.Invoke();
        moveSpeed = 1;
        isPlaying = true;
        soundManager.PlayAudio("Gameplay");
        uiManager.Gameplay_UI();
    }

    private void PlayerDied()
    {
        onGameOver.Invoke();
        moveSpeed = 0;
        isPlaying = false;
        scoreManager.UpdateText();
        upgradeManager.UpdateAllButtons();
        uiManager.Upgrades_UI();
    }

    private void Pause()
    {
        gamePaused.Invoke();
        moveSpeed = 0;
        isPlaying = false;
        uiManager.Pause_UI();
    }

    private void Options()
    {
        uiManager.Options_UI();
    }

    private void GameWin()
    {
        onPlayerWin.Invoke();
        isPlaying = false;
        uiManager.GameOver_UI();
    }


    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}

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


    public enum GameState { MainMenu, Gameplay, Upgrades, Pause, Options, GameOver };
    public GameState state;
    private GameState currentState;
    [SerializeField] private GameState beforeOptions;

    [Header("Game Values")]
    public float moveSpeed;
    internal bool isPlaying;

    internal UnityEvent onPlay = new UnityEvent();
    internal UnityEvent onGameOver = new UnityEvent();
    internal UnityEvent onPlayerWin = new UnityEvent();

    // player
    [SerializeField] private GameObject player;


    private void Start()
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
        else if(stateName == "beforeOptions")
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
        ResetRun();
        state = _state;
        currentState = state;

        switch (state)
        {
            case GameState.MainMenu: MainMenu(); break;
            case GameState.Gameplay: Gameplay(); break;
            case GameState.Upgrades: PlayerDied(); break;
            case GameState.Options: Options(); break;
            case GameState.Pause: Pause(); break;
            case GameState.GameOver: GameWin(); break;
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
        PlayerController.instance.ActiveSprite(false);
        soundManager.PlayAudio("MainMenu");
        uiManager.MainMenu_UI();
    }

    private void Gameplay()
    {
        onPlay.Invoke();
        isPlaying = true;
        PlayerController.instance.ActiveSprite(true);
        soundManager.PlayAudio("Gameplay");
        uiManager.Gameplay_UI();
    }

    private void PlayerDied()
    {
        isPlaying = false;
        onGameOver.Invoke();
        scoreManager.UpdateText();
        upgradeManager.UpdateAllButtons();
        uiManager.Upgrades_UI();
    }

    private void Pause()
    {
        uiManager.Pause_UI();
    }

    private void Options()
    {
        uiManager.Options_UI();
    }

    private void GameWin()
    {
        
    }


    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    private void IsGamePaused(bool game)
    {
        if(game)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    private void ResetRun()
    {
        if (currentState == GameState.Upgrades)
        {
            // Stop the current spawning coroutines
            // Clear existing obstacles/apples from the screen
            // Reset the health and score
            healthSystem.ResetHealthStats();
            scoreManager.ResetRun();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    [SerializeField] private UiManager uiManager;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ObstacleSpawner obstacleSpawner;

    public enum GameState { MainMenu, Gameplay, Upgrades, Pause, Options, GameEnd };
    public GameState state;
    private GameState currentState;
    private GameState beforeOptions;

    [Header("Game Values")]
    public int moveSpeed;

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


        // TO BE USED FOR DEBUGGING ONLY
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu": SetState(GameState.MainMenu); break;
            case "Gameplay": SetState(GameState.Gameplay); break;
            case "Upgrades": SetState(GameState.Upgrades); break;
            case "GameEnd": SetState(GameState.GameEnd); break;
        }
    }


    public void LoadState(string stateName)
    {
        if (Enum.TryParse(stateName, out GameState gamestate))
            LoadState(gamestate);
        else
            Debug.LogError(stateName + " doesn't exist");
    }
    private void LoadState(GameState state)
    {
        if (state == GameState.Options)
            beforeOptions = state;

        SetState(state);
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
            case GameState.Upgrades: Upgrades(); break;
            case GameState.Pause: Pause(); break;
            case GameState.GameEnd: GameEnd(); break;
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
        uiManager.MainMenu_UI();
        player.SetActive(false);
    }

    private void Gameplay()
    {
        IsGamePaused(false);
        uiManager.Gameplay_UI();
        player.SetActive(true);
    }

    private void Upgrades()
    {
        IsGamePaused(true);
        uiManager.Upgrades_UI();
        player.SetActive(false);
    }

    private void Pause()
    {
        IsGamePaused(true);
        uiManager.Pause_UI();
    }

    private void GameEnd()
    {
        uiManager.GameEnd_UI();
        player.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Game");

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
            obstacleSpawner.StopRoutines();
            obstacleSpawner.StartSpawning();
            healthSystem.ResetHealthStats();
            scoreManager.ResetRun();
        }

    }
}

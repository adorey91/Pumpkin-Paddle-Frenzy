using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    [SerializeField] private UiManager uiManager;

    public enum GameState { MainMenu, Gameplay, Pause, GameEnd };
    public GameState state;
    private GameState currentState;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    private void SetState(GameState _state)
    {
        state = _state;

        switch(state)
        {
            case GameState.MainMenu:

            break;
            case GameState.Gameplay:

            break;
            case GameState.Pause:

            break;
            case GameState.GameEnd:

            break;
        }
    }

    public void EscapeState()
    {
        switch(state)
        {
            case GameState.Pause: SetState(GameState.Gameplay); break;
            case GameState.Gameplay: SetState(GameState.Pause); break;
        }
    }

    private void MainMenu()
    {
        uiManager.MainMenu_UI();
    }

    private void Gameplay()
    {
        uiManager.Gameplay_UI();
    }

    private void Upgrade()
    {
        uiManager.Upgrades_UI();
    }

    private void Pause()
    {
        uiManager.Pause_UI();
    }

    private void GameEnd()
    {
        uiManager.GameEnd_UI();
    }
}

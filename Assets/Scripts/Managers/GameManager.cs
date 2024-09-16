using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    }

    private void Gameplay()
    {

    }

    private void Pause()
    {

    }

    private void GameEnd()
    {

    }
}

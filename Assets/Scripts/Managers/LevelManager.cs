using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{



    public void LoadScene(string sceneName)
    {
        switch(sceneName)
        {
            case "MainMenu": GameManager.instance.LoadState("MainMenu"); break;
            case "Gameplay": GameManager.instance.LoadState("Gameplay"); break;
            case "GameEnd": GameManager.instance.LoadState("GameEnd"); break;
        }
        SceneManager.LoadScene(sceneName);
    }



}

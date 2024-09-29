using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;
    internal List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private void Start()
    {
        Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        LoadAsync(sceneName);
        switch (sceneName)
        {
            case "MainMenu":
                GameManager.instance.LoadState("MainMenu"); break;
            case "Gameplay":
                GameManager.instance.LoadState("Gameplay"); break;
            case "GameEnd":
                GameManager.instance.LoadState("GameEnd"); break;
        }
    }

    private void LoadAsync(string sceneName)
    {
        UiManager.instance.UILoadingScreen(); // Show loading screen first
        StartCoroutine(LoadSceneAsync(sceneName)); // Load the scene asynchronously
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(UiManager.instance.fadeTime); // Wait for fade time
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.completed += OperationCompleted;
        scenesToLoad.Add(operation);

        // Wait until the loading is complete
        while (!operation.isDone)
        {
            yield return null; // Wait until the scene is done loading
        }
    }

    private IEnumerator WaitForScreenLoad(string sceneName)
    {
        yield return new WaitForSeconds(UiManager.instance.fadeTime);
        Debug.Log("Loading Scene Starting");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.completed += OperationCompleted;
        scenesToLoad.Add(operation);
    }

    public float GetLoadingProgress()
    {
        float totalprogress = 0;

        foreach (AsyncOperation operation in scenesToLoad)
        {
            totalprogress += operation.progress;
        }

        return totalprogress / scenesToLoad.Count;
    }

    private void OperationCompleted(AsyncOperation operation)
    {
        scenesToLoad.Remove(operation);
        operation.completed -= OperationCompleted;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private string _sceneName;
    internal List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private void OnEnable()
    {
        Actions.OnGameWin += LoadGameOver;
        Actions.LoadScene += LoadScene;
    }

    private void OnDisable()
    {
        Actions.OnGameWin -= LoadGameOver;
        Actions.LoadScene -= LoadScene;
    }

    private void LoadGameOver()
    {
        Actions.OnPlaySFX("Victory");
        LoadScene("GameEnd");
    }

    private void LoadScene(string sceneName)
    {
        _sceneName = sceneName;
        LoadAsync(sceneName);
    }

    private void LoadAsync(string sceneName)
    {
        Actions.OnScreenLoad();
        StartCoroutine(WaitForScreenLoad(sceneName)); // Load the scene asynchronously
    }

    private IEnumerator WaitForScreenLoad(string sceneName)
    {
        yield return new WaitForSecondsRealtime(LoadingScreen.fadeTime);  // Optionally, fade in loading UI
        //Debug.Log("Loading Scene Starting");

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

        GameManager.instance.LoadState(_sceneName);
    }
}
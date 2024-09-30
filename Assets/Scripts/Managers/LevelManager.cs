using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private UiManager uiManager;
    private string _sceneName;

    internal List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void Start()
    {
        GameManager.instance.onPlayerWin.AddListener(LoadGameOver);    
    }

    private void LoadGameOver()
    {
        GameManager.instance.onPlayerWin.RemoveAllListeners();
        _sceneName = "GameEnd";
        LoadAsync(_sceneName);
    }

    public void LoadScene(string sceneName)
    {
        _sceneName = sceneName;
        LoadAsync(sceneName);
    }

    private void LoadAsync(string sceneName)
    {
        uiManager.UILoadingScreen(); // Show loading screen first
        StartCoroutine(WaitForScreenLoad(sceneName)); // Load the scene asynchronously
    }

    private IEnumerator WaitForScreenLoad(string sceneName)
    {
        yield return new WaitForSeconds(uiManager.fadeTime);  // Optionally, fade in loading UI
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

        // After the scene is loaded, let GameManager handle UI transitions
       
        GameManager.instance.LoadState(_sceneName);
        Debug.Log(_sceneName);
    }
}

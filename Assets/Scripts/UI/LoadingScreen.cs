using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private LevelManager levelManager;

    [Header("Loading Screen Elements")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private CanvasGroup loadingScreenCG;
    [SerializeField] private Image loadingBar;
    public static float fadeTime = 0.5f;

    private void OnEnable()
    {
        Actions.OnScreenLoad += UILoadingScreen;
    }

    private void OnDisable()
    {
        Actions.OnScreenLoad -= UILoadingScreen;
    }

    public void UILoadingScreen()
    {
        StartCoroutine(LoadingUIFadeIN());
    }

    private IEnumerator LoadingUIFadeOut()
    {
        float timer = 0;

        while (timer < fadeTime)
        {
            loadingScreenCG.alpha = Mathf.Lerp(1, 0, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        loadingScreenCG.alpha = 0;
        loadingScreen.SetActive(false);
        loadingBar.fillAmount = 0;
    }

    private IEnumerator LoadingUIFadeIN()
    {
        float timer = 0;
        loadingScreen.SetActive(true);

        while (timer < fadeTime)
        {
            loadingScreenCG.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        loadingScreenCG.alpha = 1;

        StartCoroutine(LoadingBarProgress());
    }

    private IEnumerator LoadingBarProgress()
    {
        while (levelManager.scenesToLoad.Count <= 0)
        {
            //waiting for loading to begin
            yield return null;
        }
        while (levelManager.scenesToLoad.Count > 0)
        {
            loadingBar.fillAmount = levelManager.GetLoadingProgress();
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(LoadingUIFadeOut());
    }
}
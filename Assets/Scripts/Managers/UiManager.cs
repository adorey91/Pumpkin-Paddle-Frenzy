using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [Header("Ui Panels")]
    [SerializeField] private GameObject ui_MainMenu;
    [SerializeField] private GameObject ui_Pause;
    [SerializeField] private GameObject ui_Options;
    [SerializeField] private GameObject ui_Upgrades;
    [SerializeField] private GameObject ui_Instructions;
    [SerializeField] private GameObject ui_Gameplay;
    [SerializeField] private GameObject ui_GameEnd;

    [Header("Text Objects")]
    [SerializeField] private TMP_Text endSceneText;

    [Header("Loading Screen Elements")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private CanvasGroup loadingScreenCG;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    public float fadeTime = 0.5f;

    [SerializeField] private GameObject onScreenButtons;
    [SerializeField] private bool activeButtons = true;


    private void Start()
    {
        instance = this;
    }

    public void MainMenu_UI() => SetActiveUI(ui_MainMenu);
    public void Pause_UI() => SetActiveUI(ui_Pause);
    public void Options_UI() => SetActiveUI(ui_Options);
    public void Gameplay_UI() => SetActiveUI(ui_Gameplay);
    public void GameOver_UI() => SetActiveUI(ui_GameEnd);
    public void Upgrades_UI() => SetActiveUI(ui_Upgrades);
    public void Instructions_UI() => SetActiveUI(ui_Instructions);

    private void SetActiveUI(GameObject activeUI)
    {
        ui_MainMenu.SetActive(false);
        ui_Pause.SetActive(false);
        ui_Gameplay.SetActive(false);
        ui_GameEnd.SetActive(false);
        ui_Upgrades.SetActive(false);
        ui_Options.SetActive(false);
        ui_Instructions.SetActive(false);

        activeUI.SetActive(true);
    }

    public void ActiveScreenButtons()
    {
        activeButtons = !activeButtons;
        onScreenButtons.SetActive(activeButtons);
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
        while (LevelManager.Instance.scenesToLoad.Count <= 0)
        {
            //waiting for loading to begin
            yield return null;
        }
        while (LevelManager.Instance.scenesToLoad.Count > 0)
        {
            loadingBar.fillAmount = LevelManager.Instance.GetLoadingProgress();
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(LoadingUIFadeOut());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private LevelManager levelManager;

    [Header("Ui Panels")]
    [SerializeField] private GameObject ui_MainMenu;
    [SerializeField] private GameObject ui_Pause;
    [SerializeField] private GameObject ui_Options;
    [SerializeField] private GameObject ui_Upgrades;
    [SerializeField] private GameObject ui_Instructions;
    [SerializeField] private GameObject ui_Gameplay;
    [SerializeField] private GameObject ui_GameEnd;
    [SerializeField] private GameObject ui_Confirmation;

    [Header("Text Objects")]
    [SerializeField] private TMP_Text endSceneText;

    [Header("Loading Screen Elements")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private CanvasGroup loadingScreenCG;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    public float fadeTime = 0.5f;

    [Header("On Screen Buttons")]
    [SerializeField] private GameObject onScreenControlsButtons;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private bool activeControls = true;
    [SerializeField] private bool activePause = true;

    [Header("Confirmation Panel")]
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;


    public void MainMenu_UI() => SetActiveUI(ui_MainMenu);
    public void Pause_UI() => SetActiveUI(ui_Pause);
    public void Options_UI() => SetActiveUI(ui_Options);
    public void Gameplay_UI() => SetActiveUI(ui_Gameplay);
    public void GameOver_UI() => SetActiveUI(ui_GameEnd);
    public void Upgrades_UI() => SetActiveUI(ui_Upgrades);
    public void Instructions_UI() => SetActiveUI(ui_Instructions);

    public void Confirmation_UI(string name)
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        SetActiveUI(ui_Confirmation);

        switch(name)
        {
            case "save":
                confirmText.text = "Do you want to load previous save?";
                yesButton.onClick.AddListener(saveManager.Load);
                noButton.onClick.AddListener(Instructions_UI);
                break;
            case "quit":
                confirmText.text = "Are you sure you want to quit?";
                yesButton.onClick.AddListener(() => GameManager.instance.Quit());
                noButton.onClick.AddListener(() => GameManager.instance.LoadState("beforeOptions"));
                break;
            case "mainmenu":
                confirmText.text = "Are you sure you want to go to Main Menu? All progress will not be saved";
                yesButton.onClick.AddListener(() => levelManager.LoadScene("MainMenu"));
                noButton.onClick.AddListener(() => GameManager.instance.LoadState("beforeOptions"));
                break;
            default:
                confirmText.text = "Yes button will not work.";
                break;
        }
    }

    private void SetActiveUI(GameObject activeUI)
    {
        ui_MainMenu.SetActive(false);
        ui_Pause.SetActive(false);
        ui_Gameplay.SetActive(false);
        ui_GameEnd.SetActive(false);
        ui_Upgrades.SetActive(false);
        ui_Options.SetActive(false);
        ui_Instructions.SetActive(false);
        ui_Confirmation.SetActive(false);

        activeUI.SetActive(true);
    }

    public void ActiveOnScreenButtons()
    {
        activeControls = !activeControls;
        onScreenControlsButtons.SetActive(activeControls);
    }

    public void ActivePauseButton()
    {
        activePause = !activePause;
        pauseButton.SetActive(activePause);
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

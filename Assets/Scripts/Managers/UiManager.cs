using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] protected UpgradeManager upgradeManager;

    [Header("Ui Panels")]
    [SerializeField] private GameObject ui_MainMenu;
    [SerializeField] private GameObject ui_Pause;
    [SerializeField] private GameObject ui_Options;
    [SerializeField] private GameObject ui_Upgrades;
    [SerializeField] private GameObject ui_Gameplay;
    [SerializeField] private GameObject ui_GameEnd;
    [SerializeField] private GameObject ui_Confirmation;
    [SerializeField] private GameObject ui_Results;

    [Header("Instruction UI")]
    [SerializeField] private GameObject ui_backgroundInstruct;
    [SerializeField] private GameObject ui_Instructions;
    [SerializeField] private GameObject ui_HowToPlay;
    [SerializeField] private GameObject ui_HowToPlay2;
    [SerializeField] private GameObject ui_Controls;

    [Header("On Screen Buttons")]
    [SerializeField] private GameObject onScreenControlsButtons;
    [SerializeField] private GameObject pauseButton;
    public bool activeControls = true;
    public bool activePause = true;

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
    public void Results_UI() => SetActiveUI(ui_Results);
    public void Confirmation_UI(string name)
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        SetActiveUI(ui_Confirmation);

        switch (name)
        {
            case "save":
                SetConfirmation($"Do you want to load previous save? \nIf not, current save will be deleted",
                    () =>
                    {
                        saveManager.Load();
                        ui_backgroundInstruct.SetActive(false);
                    },
                    () =>
                    {
                        Instructions_UI();
                        saveManager.DeleteSave();
                    }
                    ); break;
            case "quit":
                SetConfirmation("Are you sure you want to quit?",
                    () => GameManager.instance.Quit(),
                    () => GameManager.instance.LoadState("beforeOptions")
                ); break;
            case "mainmenu":
                SetConfirmation("Are you sure you want to go to Main Menu?",
                    () => levelManager.LoadScene("MainMenu"),
                    () => GameManager.instance.LoadState("beforeOptions")
                ); break;
            default: confirmText.text = "Yes button will not work."; break;
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
        ui_HowToPlay.SetActive(false);
        ui_HowToPlay2.SetActive(false);
        ui_Controls.SetActive(false);
        ui_Results.SetActive(false);
        ui_backgroundInstruct.SetActive(false);

        if (activeUI == ui_Instructions)
            ui_backgroundInstruct.SetActive(true);

        activeUI.SetActive(true);
    }

    private void SetConfirmation(string message, UnityEngine.Events.UnityAction yesAction, UnityEngine.Events.UnityAction noAction)
    {
        confirmText.text = message;
        yesButton.onClick.AddListener(yesAction);
        noButton.onClick.AddListener(noAction);
    }

    // Do I need to set the toggles to whatever the onscreen is?
    public void LoadButtons()
    {
        onScreenControlsButtons.SetActive(activeControls);
        pauseButton.SetActive(activePause);
    }

    private void OnEnable()
    {
        Actions.LoadSettings += LoadButtons;
    }

    private void OnDisable()
    {
        Actions.LoadSettings -= LoadButtons;
    }

}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
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
    [SerializeField] private GameObject ui_BackgroundInstruct;
    [SerializeField] private GameObject ui_Instructions;
    [SerializeField] private GameObject ui_HowToPlay;
    [SerializeField] private GameObject ui_HowToPlay2;
    [SerializeField] private GameObject ui_HowToControls;

    [Header("Confirmation Panel")]
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Options UI")]
    [SerializeField] private GameObject ui_Audio;
    [SerializeField] private GameObject ui_Video;
    [SerializeField] private GameObject ui_Controls;
    [SerializeField] private GameObject ui_Credits;


    public void MainMenu_UI() => SetActiveUI(ui_MainMenu);
    public void Pause_UI() => SetActiveUI(ui_Pause);
    public void Options_UI() => SetActiveUI(ui_Options);
    public void Gameplay_UI() => SetActiveUI(ui_Gameplay);
    public void GameOver_UI() => SetActiveUI(ui_GameEnd);
    public void Upgrades_UI() => SetActiveUI(ui_Upgrades);
    public void Instructions_UI() => SetActiveUI(ui_Instructions);
    public void Results_UI() => SetActiveUI(ui_Results);
    public void Credits_UI() => SetActiveUI(ui_Credits);
    public void Audio_UI() => SetActiveUI(ui_Audio);
    public void Video_UI() => SetActiveUI(ui_Video);
    public void Controls_UI() => SetActiveUI(ui_Controls);

    public void Confirmation_UI(string name)
    {
        // Sets active uI to confirmation
        SetActiveUI(ui_Confirmation);

        // Removes all listeners from yes and no buttons
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();


        switch (name)
        {
            case "save":
                SetConfirmation($"Do you want to load previous save? \nIf not, current save will be deleted",
                    () =>
                    {
                        SetActiveUI(null);
                        Actions.LoadSave();
                    },
                    () =>
                    {
                        Instructions_UI();
                        Actions.DeleteSave();
                    }
                    ); break;
            case "quit":
                SetConfirmation("Are you sure you want to quit?",
                    () => GameManager.instance.Quit(),
                    () => GameManager.instance.LoadState("beforeOptions")
                ); break;
            case "mainmenu":
                SetConfirmation("Are you sure you want to go to Main Menu?",
                    () => Actions.LoadScene("MainMenu"),
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
        ui_HowToControls.SetActive(false);
        ui_Results.SetActive(false);
        ui_BackgroundInstruct.SetActive(false);
        ui_Credits.SetActive(false);
        ui_Audio.SetActive(false);
        ui_Video.SetActive(false);
        ui_Controls.SetActive(false);


        if (activeUI == ui_Instructions)
            ui_BackgroundInstruct.SetActive(true);
        
        if(activeUI != null)       
            activeUI.SetActive(true);
    }

    // Sets confirmation text, yes button action and no button action
    private void SetConfirmation(string message, UnityEngine.Events.UnityAction yesAction, UnityEngine.Events.UnityAction noAction)
    {
        confirmText.text = message;
        yesButton.onClick.AddListener(yesAction);
        noButton.onClick.AddListener(noAction);
    }
}
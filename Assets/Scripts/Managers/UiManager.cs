using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("Ui Panels")]
    [SerializeField] private GameObject ui_MainMenu;
    [SerializeField] private GameObject ui_Pause;
    [SerializeField] private GameObject ui_Gameplay;
    [SerializeField] private GameObject ui_GameEnd;


    public void MainMenu_UI()
    {
        SetActiveUI(ui_MainMenu);
    }

    public void Pause_UI()
    {
        SetActiveUI(ui_Pause);
    }

    public void Gameplay_UI()
    {
        SetActiveUI(ui_Gameplay);
    }

    public void GameEnd_UI()
    {
        SetActiveUI(ui_GameEnd);
    }

    private void SetActiveUI(GameObject activeUI)
    {
        ui_MainMenu.SetActive(false);
        ui_Pause.SetActive(false);
        ui_Gameplay.SetActive(false);
        ui_GameEnd.SetActive(false);

        activeUI.SetActive(true);
    }
}

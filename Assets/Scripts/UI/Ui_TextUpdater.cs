using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// This class is used to update Text UI - keeping it separate from the managers
/// </summary>
public class Ui_TextUpdater : MonoBehaviour
{
    [Header("Tracker of Spawn Level Progress")]
    [SerializeField] private TMP_Text levelProgressText;

    [Header("Score Text")]
    [SerializeField] private TMP_Text runResultsText;
    [SerializeField] private TMP_Text bestRunText;
    [SerializeField] private TMP_Text bestRunMenuText;

    [SerializeField] private TMP_Text applesThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptNumber;


    #region EnableDisable
    private void OnEnable()
    {
        Actions.UpdateAttemptText += UpdateAttempt;
        Actions.UpdateAppleText += UpdateAppleCount;
        Actions.UpdateResultsText += RunResultsText;
        Actions.LevelChange += LevelProgressText;
        Actions.ChangeEndlessVisibility += ProgressVisibilty;
        Actions.UpdateMenuBestRun += SetMenuRunTime;
    }

    private void OnDisable()
    {
        Actions.UpdateAttemptText -= UpdateAttempt;
        Actions.UpdateAppleText -= UpdateAppleCount;
        Actions.UpdateResultsText -= RunResultsText;
        Actions.LevelChange -= LevelProgressText;
        Actions.ChangeEndlessVisibility -= ProgressVisibilty;
        Actions.UpdateMenuBestRun -= SetMenuRunTime;
    }
    #endregion


    public void LevelProgressText(int level)
    {
        float progress = ((float)level / (float)GameManager.instance.winningLevel) * 100;
        int roundedProgress = Mathf.RoundToInt(progress);

        levelProgressText.text = $"Level Progress: {roundedProgress}%";
    }


    public void UpdateAttempt(int attempt)
    {
        attemptNumber.text = $"Attempt #: {attempt}";
    }

    public void UpdateAppleCount(int appleCount, int totalApple)
    {
        applesThisRun.text = $"x {appleCount}";
        totalApples.text = $"x {totalApple}";
    }

    public void RunResultsText(TimeSpan time, TimeSpan bestTime, bool newBestRun, int appleCount)
    {
        if(GameManager.instance.gameIsEndless)
            SetRunTime(bestTime, newBestRun);

        string appleText = appleCount == 1 ? "apple" : "apples";

        runResultsText.text = $"You survived for {time.Minutes:D2}:{time.Seconds:D2} \nCollected {appleCount} {appleText} this run!";
    }

    private void SetRunTime(TimeSpan bestTime, bool newBestRun)
    {
        if(GameManager.instance.gameIsEndless)
        {
            bestRunText.enabled = true;
            if (newBestRun)
            {
                bestRunText.text = $"<color=green>New</color> Best Time: {bestTime.Minutes:D2}:{bestTime.Seconds:D2}";
                SetMenuRunTime(bestTime);
            }
            else
                bestRunText.text = $"Best Time: {bestTime.Minutes:D2}:{bestTime.Seconds:D2}";

            //Debug.Log("Set best time");
            newBestRun = false;
        }
        else
            bestRunText.enabled = false;

    }

    private void SetMenuRunTime(TimeSpan bestTime)
    {
        bestRunMenuText.text = $"Best Time in Endless Mode: {bestTime.Minutes:D2}:{bestTime.Seconds:D2}";
    }

    private void ProgressVisibilty(string endless)
    {
        switch (endless)
        {
            case "Enable": levelProgressText.enabled = true; break;
            case "Disable": levelProgressText.enabled = false; break;
        }
    }
}
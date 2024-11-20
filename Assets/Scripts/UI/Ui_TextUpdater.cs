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
    [Header("Score Text")]
    [SerializeField] private TMP_Text runResultsText;
    [SerializeField] private TMP_Text bestRunText;
    [SerializeField] private TMP_Text bestRunMenuText;

    [SerializeField] private TMP_Text applesThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptNumber;

    [SerializeField] private TMP_Text gameEndResults;

    public static int _AppleTotalEndGame;
    private int _AttemptTotalEndGame;
    private int _CrashAmountsEndGame;


    private void Start()
    {
        _AppleTotalEndGame = 0;
    }

    #region EnableDisable
    private void OnEnable()
    {
        Actions.UpdateAttemptText += UpdateAttempt;
        Actions.UpdateAppleText += UpdateAppleCount;
        Actions.UpdateResultsText += RunResultsText;
        Actions.UpdateMenuBestRun += SetMenuRunTime;
        Actions.OnGameWin += SetEndScreenText;
    }

    private void OnDisable()
    {
        Actions.UpdateAttemptText -= UpdateAttempt;
        Actions.UpdateAppleText -= UpdateAppleCount;
        Actions.UpdateResultsText -= RunResultsText;
        Actions.UpdateMenuBestRun -= SetMenuRunTime;
        Actions.OnGameWin -= SetEndScreenText;
    }
    #endregion

    public void UpdateAttempt(int attempt)
    {
        if (!GameManager.instance.gameIsEndless)
        {
            attemptNumber.text = $"Attempt #: {attempt}";
            attemptNumber.enabled = true;
        }
        else
        {
            attemptNumber.enabled = false;
        }

        _AttemptTotalEndGame = attempt;
    }

    public void UpdateAppleCount(int appleCount, int totalApple)
    {
        applesThisRun.text = $"x {appleCount}";
        totalApples.text = $"x {totalApple}";

        
    }

    public void RunResultsText(TimeSpan time, TimeSpan bestTime, bool newBestRun, int appleCount)
    {
        if (GameManager.instance.gameIsEndless)
            SetRunTime(bestTime, newBestRun);
        else
            bestRunText.enabled = false;

        string appleText = appleCount == 1 ? "apple" : "apples";

        runResultsText.text = $"You survived for {time.Minutes:D2}:{time.Seconds:D2} \nCollected {appleCount} {appleText} this run!";
    }

    private void SetEndScreenText()
    {
        gameEndResults.text = $"Stats:\nYou collected {_AppleTotalEndGame} apples overall \nYou finished in {_AttemptTotalEndGame} attempts";
    }

    private void SetRunTime(TimeSpan bestTime, bool newBestRun)
    {
        if (GameManager.instance.gameIsEndless)
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
    }

    private void SetMenuRunTime(TimeSpan bestTime)
    {
        bestRunMenuText.text = $"Best Time in Endless Mode: {bestTime.Minutes:D2}:{bestTime.Seconds:D2}";
    }
}
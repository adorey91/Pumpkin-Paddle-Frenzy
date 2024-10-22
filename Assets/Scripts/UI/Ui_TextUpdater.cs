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
    [SerializeField] private GameObject levelProg;
    [SerializeField] private TMP_Text levelProgressText;

    [Header("Score Text")]
    [SerializeField] private TMP_Text runResults;
    [SerializeField] private TMP_Text bestRunResults;
    [SerializeField] private TMP_Text bestResultsMainMenu;

    [SerializeField] private TMP_Text applesThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptNumber;

    [SerializeField] private Stats bestRunStat;

    private void Start()
    {
        SetRunTime();
    }

    private void OnEnable()
    {
        Actions.UpdateAppleText += AppleCountText;
        Actions.OnGameOver += AppleCountText;
        Actions.OnGameOver += RunResultsText;
        Actions.OnGameWin += AppleCountText;
        Actions.OnLevelIncrease += LevelProgressText;
        Actions.UpdateAttemptText += AttemptText;
        Actions.OnIsEndless += Endless;
        Actions.OnNotEndless += NotEndless;
    }

    private void OnDisable()
    {
        Actions.UpdateAppleText -= AppleCountText;
        Actions.OnGameOver -= AppleCountText;
        Actions.OnGameOver -= RunResultsText;
        Actions.OnGameWin -= AppleCountText;
        Actions.OnLevelIncrease -= LevelProgressText;
        Actions.UpdateAttemptText -= AttemptText;
        Actions.OnIsEndless -= Endless;
        Actions.OnNotEndless -= NotEndless;
    }

    public void LevelProgressText()
    {
        float progress = ((float)Spawner.level / (float)Spawner.winningLevel) * 100;
        int roundedProgress = Mathf.RoundToInt(progress);

        levelProgressText.text = $"Level Progress: {roundedProgress}%";
    }


    public void AttemptText()
    {
        attemptNumber.text = $"Attempt #: {ScoreManager.attemptNumber}";
    }

    public void AppleCountText()
    {
        applesThisRun.text = $"x {ScoreManager.appleCount}";
        totalApples.text = $"x {ScoreManager.totalAppleCount}";
    }

    public void RunResultsText()
    {
        TimeSpan time = TimeSpan.FromSeconds(ScoreManager.runTime);

        SetRunTime();
        string appleText = ScoreManager.appleCount == 1 ? "apple" : "apples";

        runResults.text = $"You survived for {time.Minutes:D2}:{time.Seconds:D2} \nCollected {ScoreManager.appleCount} {appleText} this run!";
    }

    private void SetRunTime()
    {
        if (ScoreManager.runTime != 0)
            bestRunStat.SetBestTime(ScoreManager.runTime);
        else
            bestRunStat.SetBestTime(bestRunStat.timeInFloat);

        if (bestRunStat.setBestTime == true)
        {
            bestRunResults.text = $"<color=green>New</color> Best Time: {bestRunStat.bestTime.Minutes:D2}:{bestRunStat.bestTime.Seconds:D2}";
            bestResultsMainMenu.text = $"Best Time in Endless Mode: {bestRunStat.bestTime.Minutes:D2}:{bestRunStat.bestTime.Seconds:D2}";
        }
        else
            bestRunResults.text = $"Best Time: {bestRunStat.bestTime.Minutes:D2}:{bestRunStat.bestTime.Seconds:D2}";

        bestRunStat.setBestTime = false;
    }

    private void Endless() => levelProg.SetActive(false);
    private void NotEndless() => levelProg.SetActive(true);
}
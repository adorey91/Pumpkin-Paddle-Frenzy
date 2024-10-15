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
    [SerializeField] private TMP_Text runResults;
    [SerializeField] private TMP_Text applesThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptNumber;

    private void OnEnable()
    {
        Actions.UpdateAppleText += AppleCountText;
        Actions.OnGameOver += AppleCountText;
        Actions.OnGameOver += RunResultsText;
        Actions.OnGameWin += AppleCountText;
        Actions.OnLevelIncrease += LevelProgressText;
        Actions.UpdateAttemptText += AttemptText;
    }

    private void OnDisable()
    {
        Actions.UpdateAppleText -= AppleCountText;
        Actions.OnGameOver -= AppleCountText;
        Actions.OnGameOver -= RunResultsText;
        Actions.OnGameWin -= AppleCountText;
        Actions.OnLevelIncrease -= LevelProgressText;
        Actions.UpdateAttemptText -= AttemptText;
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

        string appleText = ScoreManager.appleCount == 1 ? "apple" : "apples";

        runResults.text = $"You survived for {time.Minutes:D2}:{time.Seconds:D2} \nCollected {ScoreManager.appleCount} {appleText} this run!";
    }
}
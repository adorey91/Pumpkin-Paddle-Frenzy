using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [Header("Counts needed for score")]
    private int totalAppleCount;
    private int appleCount;
    private int attemptNumber;
    private float runTime;
    private float bestRunFloat; // best run in float
    private TimeSpan bestTime; // best time in timespan
    private bool newBestRun = false;

    private void Start()
    {
        attemptNumber = 0;
        appleCount = 0;
        Actions.UpdateAttemptText(attemptNumber);
        Actions.UpdateAppleText(appleCount, totalAppleCount);
    }

    private void Update()
    {
        if (GameManager.instance.isPlaying)
            runTime += Time.deltaTime;
    }

    #region EnableDisable
    private void OnEnable()
    {
        Actions.AppleCollection += CollectApples;
        Actions.OnGameOver += Results;
        Actions.OnGameplay += ResetRun;
    }

    private void OnDisable()
    {
        Actions.AppleCollection -= CollectApples;
        Actions.OnGameOver -= Results;
        Actions.OnGameplay -= ResetRun;
    }
    #endregion

    public void CollectApples(string apple)
    {
        int value = 0;
        switch (apple)
        {
            case "apple": value = 1; break;
            case "golden": value = 3; break;
            default: value = 0; break;

        }
        appleCount += value;
        totalAppleCount += value;
        Actions.OnPlaySFX("Collection");
        Actions.UpdateAppleText(appleCount, totalAppleCount);
    }

    /// <summary>
    /// Resets apples this run 
    /// </summary>
    public void ResetRun()
    {
        appleCount = 0;
        runTime = 0;
        attemptNumber++;
        Actions.UpdateAppleText(appleCount, totalAppleCount);
    }

    public void BuyUpgrade(int cost)
    {
        totalAppleCount -= cost;
        Actions.UpdateAppleText(appleCount, totalAppleCount);
    }

    public void Results()
    {
        if(runTime >= bestRunFloat && runTime != 0)
        {
            bestRunFloat = runTime;
            bestTime = TimeSpan.FromSeconds(runTime);
            newBestRun = true;
        }
        TimeSpan runTimeSpan = TimeSpan.FromSeconds(runTime);

        Actions.UpdateResultsText(runTimeSpan, bestTime, newBestRun, appleCount);
        Actions.UpdateAttemptText(attemptNumber);

        newBestRun = false;
    }

    public float GetBestRun()
    {
        return bestRunFloat;
    }

    public void SetBestRun(float value)
    {
        bestRunFloat = value;
        bestTime = TimeSpan.FromSeconds(bestRunFloat);
        Actions.UpdateMenuBestRun(bestTime);
    }

    public int GetTotalAppleCount()
    {
        return totalAppleCount;
    }

    public void SetTotalAppleCount(int value)
    {
        if (value >= 0)
            totalAppleCount = value;
        else
            totalAppleCount = 0;
    }

    public int GetAttemptCount()
    {
        return attemptNumber;
    }

    public void SetAttempt(int attempt)
    {
        attemptNumber = attempt;
    }    
}

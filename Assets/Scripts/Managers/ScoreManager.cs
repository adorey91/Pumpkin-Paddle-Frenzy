using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [Header("Counts needed for score")]
    
    private int currentRunAppleCount;
    private int availableAppleCount;
    private int lifetimeAppleCount;

    private int attemptNumber;
    private float runTime;
    private float bestRunFloat; // best run in float
    private TimeSpan bestTime; // best time in timespan
    private bool newBestRun = false;

    private void Start()
    {
        attemptNumber = 0;
        currentRunAppleCount = 0;
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

        currentRunAppleCount += value;
        availableAppleCount += value;
        lifetimeAppleCount += value;

        Actions.OnPlaySFX("Collection");
        Actions.UpdateAppleText(currentRunAppleCount, availableAppleCount, lifetimeAppleCount);
    }

    /// <summary>
    /// Resets apples this run 
    /// </summary>
    public void ResetRun()
    {
        currentRunAppleCount = 0;
        runTime = 0;
        attemptNumber++;
        Actions.UpdateAppleText(currentRunAppleCount, availableAppleCount, lifetimeAppleCount);
    }

    public void BuyUpgrade(int cost)
    {
        availableAppleCount -= cost;
        Actions.UpdateAppleText(currentRunAppleCount, availableAppleCount, lifetimeAppleCount);
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

        Actions.UpdateResultsText(runTimeSpan, bestTime, newBestRun, currentRunAppleCount);
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
        return availableAppleCount;
    }

    public int GetLifetimeAppleCount()
    {
        return lifetimeAppleCount;
    }

    public void SetTotalAppleCount(int AvailableCount, int LifetimeCount)
    {
        if (AvailableCount >= 0)
            availableAppleCount = AvailableCount;

        lifetimeAppleCount = LifetimeCount;

        Debug.Log("Set Total Apple Count: " + availableAppleCount + "Lifetime apple count: " + lifetimeAppleCount);

        Actions.UpdateAppleText(currentRunAppleCount, availableAppleCount, lifetimeAppleCount);
    }

    public int GetAttemptCount()
    {
        return attemptNumber;
    }

    public void SetAttempt(int attempt)
    {
        attemptNumber = attempt;
        Actions.UpdateAttemptText(attemptNumber);
    }    
}

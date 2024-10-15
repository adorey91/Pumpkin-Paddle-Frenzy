using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [Header("Counts needed for score")]
    internal static int totalAppleCount;
    internal static int appleCount;
    internal static int attemptNumber;
    internal static float runTime;
    private float bestTime;

    private bool startTimer;

    private void Start()
    {
        runTime = 0;
        attemptNumber = 0;
        appleCount = 0;
        Actions.UpdateAttemptText();
        Actions.UpdateAppleText();
    }

    private void Update()
    {
        if (startTimer && GameManager.instance.isPlaying)
            runTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        Actions.OnCollectApple += CollectApples;
        Actions.OnCollectGoldenApple += CollectGoldenApples;
        Actions.OnGameplay += ResetRun;
        Actions.OnGameOver += TimeClockStart;
        Actions.OnGameWin += TimeClockStart;
    }

    private void OnDisable()
    {
        Actions.OnCollectApple -= CollectApples;
        Actions.OnCollectGoldenApple -= CollectGoldenApples;
        Actions.OnGameplay -= ResetRun;
        Actions.OnGameOver -= TimeClockStart;
        Actions.OnGameWin -= TimeClockStart;
    }

    private void TimeClockStart()
    {
        startTimer = !startTimer;
    }

    private void CollectApples()
    {
        appleCount++;
        totalAppleCount++;
        Actions.UpdateAppleText();
    }

    private void CollectGoldenApples()
    {
        appleCount += 3;
        totalAppleCount += 3;
        Actions.UpdateAppleText();
    }


    /// <summary>
    /// Resets apples this run 
    /// </summary>
    public void ResetRun()
    {
        appleCount = 0;
        runTime = 0;
        attemptNumber++;
        Actions.UpdateAttemptText();
        Actions.UpdateAppleText();
        TimeClockStart();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [Header("Counts needed for score")]
    public int appleCount;
    public int applesThisRun;
    internal int attemptNumber;
    private float currentTime;
    private float runTime;
    private float bestTime;

    private bool startTimer;
    [SerializeField] private TMP_Text runResults;


    [Header("Text Objects to show score values")]
    [SerializeField] private TMP_Text appleTextThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptText;

    private void Start()
    {
        currentTime = 0;
        attemptNumber = 0;
        attemptText.text = $"Attempts: {attemptNumber}";
        UpdateText();
    }

    private void Update()
    {
        if (startTimer && GameManager.instance.isPlaying)
            currentTime += Time.deltaTime;
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
        applesThisRun++;
        appleCount++;
        UpdateText();
    }

    private void CollectGoldenApples()
    {
        applesThisRun += 3;
        appleCount += 3;
        UpdateText();
    }


    /// <summary>
    /// Resets apples this run, text update runs another update function
    /// </summary>
    public void ResetRun()
    {
        applesThisRun = 0;
        currentTime = 0;
        UpdateText();
        UpdateAttempt();
        TimeClockStart();
    }

    /// <summary>
    /// Updates the attempts made
    /// </summary>
    public void UpdateAttempt()
    {
        attemptNumber++;
        attemptText.text = $"Attempts: {attemptNumber}";
    }

    /// <summary>
    /// Updates the ui text
    /// </summary>
    public void UpdateText()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);

        runResults.text = $"You survived for {time.Minutes}:{time.Seconds} \nCollected {applesThisRun} apples this run!";
        appleTextThisRun.text = $"x {applesThisRun}";
        totalApples.text = $"x {appleCount}";
    }
}

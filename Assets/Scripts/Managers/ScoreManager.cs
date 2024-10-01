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

    [Header("Text Objects to show score values")]
    [SerializeField] private TMP_Text appleTextThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptText;

    private void Start()
    {
        attemptNumber = 0;
        attemptText.text = $"Attempts: {attemptNumber}";
        UpdateText();

        GameManager.instance.onGameOver.AddListener(ResetRun);
    }

    /// <summary>
    /// Resets apples this run, text update runs another update function
    /// </summary>
    public void ResetRun()
    {
        applesThisRun = 0;
        UpdateText();
        UpdateAttempt();
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
        appleTextThisRun.text = $"x {applesThisRun}";
        totalApples.text = $"x {appleCount}";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int appleCount;
    public int applesThisRun;
    internal int attemptNumber;

    [SerializeField] private TMP_Text appleTextThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptText;

    private void Start()
    {
        attemptNumber = 1;
        attemptText.text = $"Attempts: {attemptNumber}";
        UpdateText();
    }

    public void ResetRun()
    {
        applesThisRun = 0;
        appleTextThisRun.text = $"x {applesThisRun}";
        UpdateAttempt();
    }

    public void UpdateAttempt()
    {
        attemptNumber++;
        attemptText.text = $"Attempts: {attemptNumber}";
    }

    public void UpdateText()
    {
        appleTextThisRun.text = $"x {applesThisRun}";
        totalApples.text = $"x {appleCount}";
    }
}

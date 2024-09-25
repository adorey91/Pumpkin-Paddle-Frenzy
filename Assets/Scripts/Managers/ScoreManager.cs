using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static int appleCount;
    public static int applesThisRun;
    private int attemptNumber;

    [SerializeField] private TMP_Text appleTextThisRun;
    [SerializeField] private TMP_Text totalApples;
    [SerializeField] private TMP_Text attemptText;

    private void Start()
    {
        Instance = this;
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

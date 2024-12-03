using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Timer", menuName = "Timer")]
public class CustomTimerSO : ScriptableObject
{
    public float duration;
    public float elapsedTime = 0f;
    public bool isRunning = false;

    public void StartTimer(float newTime)
    {
        elapsedTime = 0;
        duration = newTime;
        isRunning = true;
    }

    public bool UpdateTimer(float deltaTime)
    {
        if (!isRunning)
            return false;

        elapsedTime += deltaTime;
        if (elapsedTime >= duration)
        {
            isRunning = false;
            return true;
        }
        return false;
    }

    public float GetRemainingTime()
    {
        if (!isRunning)
            return 0;

        return Mathf.Max(duration - elapsedTime, 0);
    }

    public void ResetTimer()
    {
        StartTimer(duration);
    }
}

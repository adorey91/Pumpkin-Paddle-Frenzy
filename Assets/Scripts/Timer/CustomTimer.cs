using UnityEngine;

public class CustomTimer
{
    public float duration;
    public float elapsedTime = 0f;
    public bool isRunning = false;


    public CustomTimer(float newDuration)
    {
        this.duration = newDuration;
        this.elapsedTime = 0f;
        this.isRunning = false;
    }

    public void StartTimer(float newDuration)
    {
        elapsedTime = 0;
        this.duration = newDuration;
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

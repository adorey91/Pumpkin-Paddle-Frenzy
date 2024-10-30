using UnityEngine;

public class CustomTimer
{
    public float duration;
    public float elapedTime = 0f;
    public bool isRunning = false;


    public CustomTimer(float duration)
    {
        this.duration = duration;
        this.elapedTime = 0f;
        this.isRunning = false;
    }

    public void StartTimer(float duration)
    {
        elapedTime = 0;
        this.duration = duration;
        isRunning = true;
    }

    public bool UpdateTimer(float deltaTime)
    {
        if (!isRunning)
            return false;

        elapedTime += deltaTime;
        if (elapedTime >= duration)
        {
            isRunning = false;
            return true;
        }
        return false;
    }

    public void ResetTimer()
    {
        elapedTime = 0;
        isRunning = false;
    }
}

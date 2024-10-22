using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "GameStat")]
public class Stats : ScriptableObject
{
    public float timeInFloat = 0;
    public TimeSpan bestTime;
    public bool setBestTime;

    public void SetBestTime(float time)
    {
        if(time >= timeInFloat)
        {
            setBestTime = true;
            timeInFloat = time;
            bestTime = TimeSpan.FromSeconds(time);
        }
    }
}

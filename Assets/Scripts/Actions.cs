using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public static class Actions
{
    // Player Actions - Apple Collection, change player sprite visibility, player hurt, reset upgrade and player health stats
    public static Action<string> AppleCollection;
    public static Action<string> ChangeSpriteVisibility;
    public static Action OnPlayerHurt;
    public static Action ResetStats;

    // Menu Actions
    public static Action OnGameplay;
    public static Action OnGameWin;
    public static Action OnGameOver;
    public static Action ApplySettings;
    public static Action LoadBestRun;
    public static Action LoadSave;
    public static Action DeleteSave;
    public static Action<string> LoadScene;
    public static Action<string> ChangeEndlessVisibility;

    // UI Actions
    public static Action OnScreenLoad;
    public static Action<int> UpdateAttemptText;
    public static Action<int, int, int> UpdateAppleText;
    public static Action<TimeSpan, TimeSpan, bool, int> UpdateResultsText;
    public static Action<TimeSpan> UpdateMenuBestRun;
    public static Action<float> SpeedChange;

    // Spawnable Actions
    public static Action<PoolType> OnSpawn;
    public static Action<PoolType, GameObject, bool> OnReturn;
    public static Action ReturnAllToPool;

    // Music Actions
    public static Action<string> OnPlaySFX;
    public static Action<string> OnPlayMusic;
}

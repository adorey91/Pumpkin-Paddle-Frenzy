using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{
    // Player Actions
    public static Action OnCollectApple;
    public static Action OnCollectGoldenApple;
    public static Action OnPlayerHurt;
    //public static Action TurnOnPlayerSprite;
    //public static Action TurnOffPlayerSprite;

    // Toggle Actions
    public static Action OnScreenControlsToggle;
    public static Action OnScreenPauseToggle;

    // Menu Actions
    public static Action OnGameplay;
    public static Action OnGamePause;
    public static Action OnGameWin;
    public static Action OnGameOver;
    public static Action LoadSettings;

    // UI Actions
    public static Action OnScreenLoad;
    public static Action UpdateAttemptText;
    public static Action UpdateAppleText;
    public static Action OnLevelIncrease;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_TextUpdater : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManager scoreManager;

    private void Start()
    {
        
    }

    public void EndOfRunText()
    {
        //TimeSpan timeTaken = TimeSpan.FromSeconds(scoreManager.currentTime);
    }
}

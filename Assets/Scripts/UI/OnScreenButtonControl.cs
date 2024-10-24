using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenButtonControl : MonoBehaviour
{
    [Header("On Screen Buttons")]
    [SerializeField] private GameObject onScreenControlsButtons;
    [SerializeField] private GameObject pauseButton;
    public bool activeControls = true;
    public bool activePause = true;

    private void Start()
    {
        LoadButtons();
    }

    public void LoadButtons()
    {
        onScreenControlsButtons.SetActive(activeControls);
        pauseButton.SetActive(activePause);
    }

    private void OnEnable()
    {
        Actions.ApplySettings += LoadButtons;
    }

    private void OnDisable()
    {
        Actions.ApplySettings -= LoadButtons;
    }
}

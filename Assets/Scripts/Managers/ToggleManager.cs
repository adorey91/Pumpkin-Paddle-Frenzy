using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleManager : MonoBehaviour
{
    public ToggleSync onScreenControlToggle;
    public ToggleSync pauseButtonToggle;

    private bool keyboardDetected;

    private void Start()
    {
        CheckForKeyboard();

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Running on Android");
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Running on Windows PC");
        }
    }

    public void CheckForKeyboard()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                Debug.Log("Running on Windows Player");
                keyboardDetected = true;
                break;
            case RuntimePlatform.Android:
                Debug.Log("Running on Android");
                keyboardDetected = false;
                break;
        }

        SetOnScreenControlsState(!keyboardDetected);
        SetPauseButtonState(!keyboardDetected);
    }


    // Method to set the OnScreenControls toggle state
    public void SetOnScreenControlsState(bool state)
    {
        if (onScreenControlToggle != null && onScreenControlToggle.currentToggle != null)
        {
            onScreenControlToggle.currentToggle.isOn = state;
            onScreenControlToggle.onScreenObject.SetActive(state);
        }
    }

    // Method to set the PauseButton toggle state
    public void SetPauseButtonState(bool state)
    {
        if (pauseButtonToggle != null && pauseButtonToggle.currentToggle != null)
        {
            pauseButtonToggle.currentToggle.isOn = state;
            pauseButtonToggle.onScreenObject.SetActive(state);
        }
    }

    // Return the state of the pause button toggle
    public bool GetOnScreenControlsState()
    {
        return onScreenControlToggle != null && onScreenControlToggle.currentToggle != null
         ? onScreenControlToggle.currentToggle.isOn
         : false;
    }

    // Return the state of the pause button toggle
    public bool GetPauseButtonState()
    {
        return pauseButtonToggle != null && pauseButtonToggle.currentToggle != null
       ? pauseButtonToggle.currentToggle.isOn
       : false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleManager : MonoBehaviour
{
    public ToggleSync onScreenControlToggle;
    public ToggleSync pauseButtonToggle;


    private void Start()
    {
        CheckForKeyboard();
    }

    public void CheckForKeyboard()
    {
        foreach (InputDevice device in InputSystem.devices)
        {
            if (device.name == "Keyboard")
            {
                SetOnScreenControlsState(false);
                SetPauseButtonState(false);
            }
        }
    }

    // Method to set the OnScreenControls toggle state
    public void SetOnScreenControlsState(bool state)
    {
        onScreenControlToggle.SetToggleState(state);
    }

    // Method to set the PauseButton toggle state
    public void SetPauseButtonState(bool state)
    {
        pauseButtonToggle.SetToggleState(state);
    }

    // Get the global state of OnScreenControls toggles (all must match to return a value)
    public bool GetOnScreenControls()
    {
        return onScreenControlToggle.currentToggle.isOn;
    }

    // Get the global state of PauseButton toggles (all must match to return a value)
    public bool GetPauseButton()
    {
        return pauseButtonToggle.currentToggle.isOn;
    }
}

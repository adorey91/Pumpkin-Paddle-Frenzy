using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleManager : MonoBehaviour
{
    public ToggleSync[] onScreenControlToggles;
    public ToggleSync[] pauseButtonToggles;

    public GameObject onScreenControls;
    public GameObject pauseButton;

    // Method to set the OnScreenControls toggle state
    public void SetOnScreenControlsState(bool state)
    {
        foreach (var toggle in onScreenControlToggles)
        {
            if (toggle != null && toggle.type == ToggleSync.ToggleType.OnScreenControls)
            {
                toggle.SetToggleState(state);
            }
        }
    }

    // Method to set the PauseButton toggle state
    public void SetPauseButtonState(bool state)
    {
        foreach (var toggle in pauseButtonToggles)
        {
            if (toggle != null && toggle.type == ToggleSync.ToggleType.PauseButton)
            {
                toggle.SetToggleState(state);
            }
        }
    }

    // Get the global state of OnScreenControls toggles (all must match to return a value)
    public bool GetOnScreenControls()
    {
        foreach (var toggle in onScreenControlToggles)
        {
            if (toggle != null && toggle.type == ToggleSync.ToggleType.OnScreenControls)
            {
                return toggle.currentToggle.isOn; // Return the state of the first OnScreenControls toggle
            }
        }
        return false; // Default if no toggles found
    }

    // Get the global state of PauseButton toggles (all must match to return a value)
    public bool GetPauseButton()
    {
        foreach (var toggle in onScreenControlToggles)
        {
            if (toggle != null && toggle.type == ToggleSync.ToggleType.PauseButton)
            {
                return toggle.currentToggle.isOn; // Return the state of the first PauseButton toggle
            }
        }
        return false; // Default if no toggles found
    }
}

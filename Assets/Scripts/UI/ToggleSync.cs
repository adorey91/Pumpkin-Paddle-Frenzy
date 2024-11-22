using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSync : MonoBehaviour
{
    public enum ToggleType
    {
        OnScreenControls,
        PauseButton
    }

    public ToggleType type;  // Define if the toggle is for OnScreenControls or PauseButton
    public Toggle currentToggle;

    public ToggleSync[] allToggles;

    private bool previousState; // Keep track of the previous state

    public GameObject onScreenControls;
    public GameObject pauseButton;

    private void Start()
    {
        // Get the Toggle component attached to this object
        if(currentToggle == null)
            currentToggle = GetComponent<Toggle>();
    }

    private void Update()
    {
        if(currentToggle.isOn != previousState)
        {
            previousState = currentToggle.isOn;
            SetToggleState(currentToggle.isOn);
        }
    }

    public void SetToggleState(bool value)
    {
        if (currentToggle != null)
        {
            previousState = value;  // Save the new state
            currentToggle.isOn = value;

            // Sync with other toggles of the same type
            foreach (var toggle in allToggles)
            {
                if (toggle != null && toggle.type == type && toggle != this)
                {
                    toggle.currentToggle.isOn = value;
                }
            }
        }

        if(type == ToggleType.OnScreenControls)
        {
            onScreenControls.SetActive(currentToggle.isOn);
        }
        
        if (type == ToggleType.PauseButton)
        {
            pauseButton.SetActive(currentToggle.isOn);
        }
    }
}

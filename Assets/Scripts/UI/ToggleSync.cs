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

    public GameObject onScreenObject;

    private void Start()
    {
        // Get the Toggle component attached to this object
        if (currentToggle == null)
            currentToggle = GetComponent<Toggle>();

        currentToggle.onValueChanged.AddListener(SetToggleState);
    }

    public void SetToggleState(bool value)
    {
        if (onScreenObject != null)
            onScreenObject.SetActive(value);
        else
            Debug.LogWarning("No object assigned to toggle.");
    }
}
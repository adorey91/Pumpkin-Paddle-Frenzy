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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

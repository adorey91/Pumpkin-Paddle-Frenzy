using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckToggle : MonoBehaviour
{
    [SerializeField] private Toggle[] onScreenControls;
    [SerializeField] private Toggle[] onScreenPause;

    private void Start()
    {
        foreach (Toggle toggle in onScreenControls)
        {
            toggle.enabled = true;
        }
        foreach (Toggle toggle in onScreenPause)
        {
            toggle.enabled = true;
        }
    }

    public void ChangeToggle()
    {

    }
}

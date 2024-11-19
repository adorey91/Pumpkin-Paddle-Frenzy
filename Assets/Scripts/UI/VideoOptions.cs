using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoOptions : MonoBehaviour
{
    [SerializeField] private List<ResItem> resolutions = new List<ResItem>();
    [SerializeField] private TMP_Text resolutionText;
    private int currentResIndex;
    private bool isFullscreen = true;

    public void Start()
    {
        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                currentResIndex = i;
                foundRes = true;
                break;
            }
        }

        if (!foundRes)
        {
            Debug.Log($"Resolution not found, creating new resolution");
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            currentResIndex = resolutions.Count - 1;
            UpdateResLabel();
        }

        ApplyGraphics();
    }

    public void ApplyGraphics()
    {
        Screen.SetResolution(resolutions[currentResIndex].horizontal, resolutions[currentResIndex].vertical, isFullscreen);
    }

    public void ResChangeLeft()
    {
        currentResIndex--;

        if (currentResIndex < 0)
            currentResIndex = resolutions.Count - 1;

        UpdateResLabel();
    }


    public void ResChangeRight()
    {
        currentResIndex++;
        if (currentResIndex >= resolutions.Count)
            currentResIndex = 0;

        UpdateResLabel();
    }

    public void FullscreenToggle()
    {
        isFullscreen = !isFullscreen;
    }

    public void UpdateResLabel()
    {
        resolutionText.text = $"{resolutions[currentResIndex].horizontal} x {resolutions[currentResIndex].vertical}";
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}

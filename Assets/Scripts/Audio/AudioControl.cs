using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]
public struct AudioControl
{
    public AudioMixerGroup mixer;
    public Image audioImage;
    public Button increaseButton;
    public Button decreaseButton;
}

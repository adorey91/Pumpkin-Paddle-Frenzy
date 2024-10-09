using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source / Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Slider")]
    [SerializeField] private Image mainAudioImage;
    [SerializeField] private Image musicAudioImage;
    [SerializeField] private Image sfxAudioImage;
    [SerializeField] private Gradient gradient;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainClip;
    [SerializeField] private AudioClip gameplayClip;
    [SerializeField] private AudioClip sfxCollectClip;
    [SerializeField] private AudioClip sfxCrashClip;
    [SerializeField] private AudioClip sfxClickButtonClip;

    public void Start()
    {
        SetVolume("Master");
        SetVolume("Music");
        SetVolume("SFX");
    }

    private void OnEnable()
    {
        Actions.OnGameOver += PlayEnemyCrash;
        Actions.OnCollectApple += PlayAppleCollection;
    }

    private void OnDisable()
    {
        Actions.OnGameOver -= PlayEnemyCrash;
        Actions.OnCollectApple -= PlayAppleCollection;
    }

    /// This should be used for looping audio.
    public void PlayAudio(string audio)
    {
        switch (audio)
        {
            case "MainMenu": musicSource.clip = mainClip; break;
            case "Gameplay": musicSource.clip = gameplayClip; break;
        }
        musicSource.loop = true;
        if(musicSource.clip == gameplayClip && !musicSource.isPlaying)
            musicSource.Play();
    }


    private void PlayEnemyCrash()
    {
        if (sfxSource == null)
        {
            GameObject foundObject = GameObject.Find("SFXSource");
            sfxSource = GetComponent<AudioSource>();
        }
            sfxSource.PlayOneShot(sfxCrashClip, 1f);
    }

    private void PlayAppleCollection()
    {
        if (sfxSource == null)
        {
            GameObject foundObject = GameObject.Find("SFXSource");
            sfxSource = GetComponent<AudioSource>();
        }
        sfxSource.PlayOneShot(sfxCollectClip, 1f);
    }

    private void PlayButtonClick()
    {
        if (sfxSource == null)
        {
            GameObject foundObject = GameObject.Find("SFXSource");
            sfxSource = GetComponent<AudioSource>();
        }
        sfxSource.PlayOneShot(sfxClickButtonClip, 1f);
    }


    // Sets volume
    public void SetVolume(string slider)
    {
        switch (slider)
        {
            case "SFX":
                audioMixer.SetFloat(slider, Mathf.Log10(sfxAudioImage.fillAmount) * 20);
                sfxAudioImage.color = gradient.Evaluate(sfxAudioImage.fillAmount);
                break;
            case "Music":
                audioMixer.SetFloat(slider, Mathf.Log10(musicAudioImage.fillAmount) * 20);
                musicAudioImage.color = gradient.Evaluate(musicAudioImage.fillAmount);
                break;
            case "Master":
                audioMixer.SetFloat(slider, Mathf.Log10(mainAudioImage.fillAmount) * 20);
                mainAudioImage.color = gradient.Evaluate(mainAudioImage.fillAmount);
                break;
            default: Debug.Log(slider + " doesnt exist"); break;
        }
    }

    // used for increase volume button
    public void IncreaseAudio(AudioMixerGroup mixerGroup)
    {
        Debug.Log(mixerGroup.name);
        switch (mixerGroup.name)
        {
            case "SFX":
                sfxAudioImage.fillAmount += 0.1f;
                SetVolume("SFX"); // Apply volume change
                break;
            case "Music":
                musicAudioImage.fillAmount += 0.1f;
                SetVolume("Music"); // Apply volume change
                break;
            case "Master":
                mainAudioImage.fillAmount += 0.1f;
                SetVolume("Master"); // Apply volume change
                break;
        }
    }

    // used for decrease volume button
    public void DecreaseAudio(AudioMixerGroup mixerGroup)
    {
        Debug.Log(mixerGroup.name);
        switch (mixerGroup.name)
        {
            case "SFX":
                sfxAudioImage.fillAmount -= 0.1f;
                SetVolume("SFX"); // Apply volume change
                break;
            case "Music":
                musicAudioImage.fillAmount -= 0.1f;
                SetVolume("Music"); // Apply volume change
                break;
            case "Master":
                mainAudioImage.fillAmount -= 0.1f;
                SetVolume("Master"); // Apply volume change
                break;
        }
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }
}

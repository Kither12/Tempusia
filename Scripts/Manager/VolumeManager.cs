using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider Masterslider, SFXSlider, MusicSlider;
    public static VolumeManager instance;

    private float currentSFX, currentMusic;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            Masterslider.value = PlayerPrefs.GetFloat("MasterVolume");
            SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume"));
        }
        else
        {
            Masterslider.value = 1;
            SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume"));
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            SetVFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }
        else
        {
            SFXSlider.value = 1;
            SetVFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }
        else
        {
            MusicSlider.value = 1;
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        currentMusic = Mathf.Log10(volume) * 20;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetVFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        currentSFX = Mathf.Log10(volume) * 20;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void ResetDefault()
    {
        SetMasterVolume(1);
        SetMusicVolume(1);
        SetVFXVolume(1);
        Masterslider.value = 1;
        SFXSlider.value = 1;
        MusicSlider.value = 1;
    }
    //set current to 100 to ensure that current only update one
    public void muteSfx()
    {
        if(currentSFX == 100)
            audioMixer.GetFloat("SFXVolume", out currentSFX);
        audioMixer.SetFloat("SFXVolume", -80);
    }
    public void unmuteSfx()
    {
        if (currentSFX != 100)
            audioMixer.SetFloat("SFXVolume", currentSFX);
        currentSFX = 100;
    }
    public void muteMusic()
    {
        if (currentMusic == 100)
            audioMixer.GetFloat("MusicVolume", out currentMusic);
        audioMixer.SetFloat("MusicVolume", -80);
    }
    public void unmuteMusic()
    {
        if (currentMusic != 100)
            audioMixer.SetFloat("MusicVolume", currentMusic);
        currentMusic = 100;
    }
}

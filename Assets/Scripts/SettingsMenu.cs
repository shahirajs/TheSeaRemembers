using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider ambienceSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (AudioManager.Instance == null) return;

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        ambienceSlider.onValueChanged.AddListener(AudioManager.Instance.SetAmbienceVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }
}

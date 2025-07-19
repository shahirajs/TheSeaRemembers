using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject prefab = Resources.Load<GameObject>("AudioManager");
                if (prefab != null)
                {
                    GameObject instanceObj = Instantiate(prefab);
                    _instance = instanceObj.GetComponent<AudioManager>();
                    Debug.Log("AudioManager auto-instantiated via Instance accessor.");
                }
                else
                {
                    Debug.LogWarning("AudioManager prefab not found in Resources folder.");
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public AudioSource musicSource;
    public AudioSource ambienceSource;
    public AudioSource sfxSource;

    public AudioClip defaultMusicClip;
    public AudioClip defaultAmbienceClip;

    public float fadeDuration = 1.5f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();
            PlayBackgroundAudioIfSilent();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void PlayBackgroundAudioIfSilent()
    {
        if (!musicSource.isPlaying && defaultMusicClip != null)
            PlayMusic(defaultMusicClip);

        if (!ambienceSource.isPlaying && defaultAmbienceClip != null)
            PlayAmbience(defaultAmbienceClip);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        StartCoroutine(CrossfadeMusic(clip, loop));
    }

    public void PlayAmbience(AudioClip clip, bool loop = true)
    {
        StartCoroutine(CrossfadeAmbience(clip, loop));
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutAndStop(musicSource));
    }

    public void StopAmbience()
    {
        StartCoroutine(FadeOutAndStop(ambienceSource));
    }

    IEnumerator CrossfadeMusic(AudioClip newClip, bool loop)
    {
        if (musicSource.clip == newClip) yield break;

        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.clip = newClip;
        musicSource.loop = loop;
        musicSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    IEnumerator CrossfadeAmbience(AudioClip newClip, bool loop)
    {
        if (ambienceSource.clip == newClip) yield break;

        float startVolume = ambienceSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            ambienceSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        ambienceSource.clip = newClip;
        ambienceSource.loop = loop;
        ambienceSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            ambienceSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        ambienceSource.volume = startVolume;
    }

    IEnumerator FadeOutAndStop(AudioSource source)
    {
        float startVolume = source.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset volume for next play
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetAmbienceVolume(float volume)
    {
        ambienceSource.volume = volume;
        PlayerPrefs.SetFloat("AmbienceVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void LoadVolumeSettings()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        ambienceSource.volume = PlayerPrefs.GetFloat("AmbienceVolume", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }
}

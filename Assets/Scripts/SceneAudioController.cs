using UnityEngine;

public class SceneAudioController : MonoBehaviour
{
    public AudioClip sceneMusic;
    public AudioClip sceneAmbience;
    public AudioClip initialSFX;

    void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager could not be initialized.");
            return;
        }

        if (sceneMusic != null)
        {
            AudioManager.Instance.PlayMusic(sceneMusic);
        }

        if (sceneAmbience != null)
        {
            AudioManager.Instance.PlayAmbience(sceneAmbience);
        }

        if (initialSFX != null)
        {
            AudioManager.Instance.PlaySFX(initialSFX);
        }
    }
}

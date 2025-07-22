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

        if (sceneMusic == null)
        {
            AudioManager.Instance.StopMusic();
            Debug.Log("SceneAudioController: Music explicitly set to none, stopping music.");
        }
        else
        {
            AudioManager.Instance.PlayMusic(sceneMusic);
            Debug.Log("SceneAudioController: Playing scene-specific music.");
        }

        if (sceneAmbience == null)
        {
            Debug.Log("SceneAudioController: Ambience not set, continuing previous ambience.");
        }
        else
        {
            AudioManager.Instance.PlayAmbience(sceneAmbience);
            Debug.Log("SceneAudioController: Playing scene-specific ambience.");
        }

        if (initialSFX != null)
        {
            AudioManager.Instance.PlaySFX(initialSFX);
            Debug.Log("SceneAudioController: Playing initial SFX.");
        }
    }
}

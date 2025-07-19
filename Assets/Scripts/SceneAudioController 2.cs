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
            GameObject audioManagerPrefab = Resources.Load<GameObject>("AudioManager");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
            }
            else
            {
                Debug.LogWarning("AudioManager prefab not found in Resources folder.");
                return;
            }
        }

        if (sceneMusic != null)
        {
            AudioManager.Instance.PlayMusic(sceneMusic);
        }
        else
        {
            AudioManager.Instance.StopMusic();
        }

        if (sceneAmbience != null)
        {
            AudioManager.Instance.PlayAmbience(sceneAmbience);
        }
        else
        {
            AudioManager.Instance.StopAmbience();
        }

        if (initialSFX != null)
        {
            AudioManager.Instance.PlaySFX(initialSFX);
        }
    }
}

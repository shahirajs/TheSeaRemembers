using UnityEngine;
using System.Collections;

public class SceneAudioController : MonoBehaviour
{
    public AudioClip sceneMusic;
    public AudioClip sceneAmbience;
    public AudioClip initialSFX;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => AudioManager.Instance != null);

        if (sceneMusic == null)
        {
            AudioManager.Instance.StopMusic();
        }
        else
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

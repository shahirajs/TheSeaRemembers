using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoAdvance : MonoBehaviour
{
    public string nextSceneName = "OpenSign";
    public float delay = 4f;

    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (dialogueManager == null)
        {

            Invoke("LoadNextScene", delay);
        }
        else if (!dialogueManager.IsDialogueActive())
        {

            Invoke("LoadNextScene", delay);
        }
        else
        {

            dialogueManager.OnDialogueComplete += LoadNextScene;
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

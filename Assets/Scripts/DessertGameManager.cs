using UnityEngine;
using UnityEngine.SceneManagement;

public class DessertGameManager : MonoBehaviour
{
    public void OnDessertComplete()
    {
        SceneManager.LoadScene(DialogueProgressManager.LastScene);
    }
}

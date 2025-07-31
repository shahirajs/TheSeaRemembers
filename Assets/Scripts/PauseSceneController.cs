using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSceneController : MonoBehaviour
{
    [SerializeField] private string thisSceneName;
    public SettingsUIManager settingsUIManager;

    public void GoToSettings()
    {
        SceneManager.LoadSceneAsync("SettingsScene2", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(thisSceneName);
    }

    public void GoToAlbum()
    {
        SceneManager.LoadSceneAsync("AlbumScene2", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(thisSceneName);
    }

    public void GoToLoad()
    {
        SceneManager.LoadSceneAsync("LoadScene2", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(thisSceneName);
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(thisSceneName);
    }

    public void GoToMainMenu()
    {
        settingsUIManager?.SendMessage("ShowConfirmation", new object[]
        {
            "Return to Main Menu? Unsaved progress will be lost.",
            (System.Action)(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            })
        });
    }
}

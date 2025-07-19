using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Opening");
    }
    public void Load()
    {
        SceneManager.LoadScene("LoadScene");
    }
    public void Album()
    {
        SceneManager.LoadScene("AlbumScene");
    }

    public void Settings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void Return()
    {
        SceneManager.LoadScene("Main Menu"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}


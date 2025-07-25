using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button albumButton;
    public Button settingsButton;
    public Button quitButton;

    void Start()
    {
        if (startButton == null)
            startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
        if (loadButton == null)
            loadButton = GameObject.Find("LoadButton")?.GetComponent<Button>();
        if (albumButton == null)
            albumButton = GameObject.Find("AlbumButton")?.GetComponent<Button>();
        if (settingsButton == null)
            settingsButton = GameObject.Find("SettingsButton")?.GetComponent<Button>();
        if (quitButton == null)
            quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();

        if (startButton != null)
            startButton.onClick.AddListener(() => SceneManager.LoadScene("Opening"));

        if (loadButton != null)
            loadButton.onClick.AddListener(() => SceneManager.LoadScene("LoadScene"));

        if (albumButton != null)
            albumButton.onClick.AddListener(() => SceneManager.LoadScene("AlbumScene"));

        if (settingsButton != null)
            settingsButton.onClick.AddListener(() => SceneManager.LoadScene("SettingsScene"));

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Button continueButton;
    public Button returnButton;
    public Button settingsButton;
    public Button quitButton;

    void Start()
    {
        if (continueButton == null)
            continueButton = GameObject.Find("ContinueButton")?.GetComponent<Button>();
        if (returnButton == null)
            returnButton = GameObject.Find("ReturnButton")?.GetComponent<Button>();
        if (settingsButton == null)
            settingsButton = GameObject.Find("SettingsButton")?.GetComponent<Button>();
        if (quitButton == null)
            quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);

        if (returnButton != null)
            returnButton.onClick.AddListener(ContinueGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(() => SceneManager.LoadScene("SettingsScene", LoadSceneMode.Additive));

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;

        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
            SceneManager.UnloadSceneAsync("PauseMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

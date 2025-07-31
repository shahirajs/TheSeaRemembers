using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsUIManager : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button albumButton;
    public Button settingsButton;
    public Button quitButton;
    public Button returnButton;

    public GameObject confirmationPopup;
    public Button confirmStartOverButton;
    public Button cancelStartOverButton;
    public TMP_Text confirmationMessageText;

    private System.Action confirmCallback;

    private void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStart);

        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoad);

        if (albumButton != null)
            albumButton.onClick.AddListener(OnAlbum);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettings);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnMainMenu);

        if (returnButton != null)
            returnButton.onClick.AddListener(OnReturn);

        if (confirmStartOverButton != null)
            confirmStartOverButton.onClick.AddListener(() =>
            {
                confirmationPopup.SetActive(false);
                confirmCallback?.Invoke();
            });

        if (cancelStartOverButton != null)
            cancelStartOverButton.onClick.AddListener(() =>
            {
                confirmationPopup.SetActive(false);
            });

        if (confirmationPopup != null)
            confirmationPopup.SetActive(false);
    }

    void ShowConfirmation(string message, System.Action onConfirm)
    {
        confirmationMessageText.text = message;
        confirmationPopup.SetActive(true);
        confirmCallback = onConfirm;
    }

    void OnStart()
    {
        ShowConfirmation("Are you sure you want to start over?", () =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Opening");
        });
    }

    void OnLoad()
    {
        SceneManager.LoadScene("LoadScene2");
    }

    void OnAlbum()
    {
        SceneManager.LoadScene("AlbumScene2");
    }

    void OnSettings()
    {
        SceneManager.LoadScene("SettingsScene2");
    }

    void OnMainMenu()
    {
        ShowConfirmation("Return to Main Menu? Unsaved progress will be lost.", () =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        });
    }

    void OnReturn()
    {
        confirmCallback = null;
        if (confirmationPopup != null)
            confirmationPopup.SetActive(false);

        Time.timeScale = 1f;

        if (SceneManager.GetSceneByName("SettingsScene2").isLoaded &&
            SceneManager.GetActiveScene().name != "SettingsScene2")
        {
            SceneManager.UnloadSceneAsync("SettingsScene2");
        }
        else
        {
            SceneManager.LoadScene(GlobalUIManager.Instance.GetPreviousScene());
        }
    }


}

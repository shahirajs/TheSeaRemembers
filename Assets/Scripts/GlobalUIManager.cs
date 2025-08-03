using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager Instance { get; private set; }

    public string[] excludedScenes = { "MainMenu", "LoadScene", "AlbumScene", "SettingsScene", "PauseMenu" };
    public string pauseMenuScene = "SettingsScene2";

    private GameObject currentMenuButton;
    private string previousScene;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        if (Instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("GlobalUIManager");
            if (prefab != null)
            {
                GameObject obj = Object.Instantiate(prefab);
                Object.DontDestroyOnLoad(obj);
            }
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += ForceSpawnButton;
            SceneManager.activeSceneChanged += ForceSpawnButtonOnSwitch;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ForceSpawnButton(Scene scene, LoadSceneMode mode)
    {
        ForceRespawnMenuButton();
    }

    void ForceSpawnButtonOnSwitch(Scene oldScene, Scene newScene)
    {
        ForceRespawnMenuButton();
    }

    void ForceRespawnMenuButton()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (IsExcluded(sceneName))
        {
            if (currentMenuButton != null)
                Destroy(currentMenuButton);
            return;
        }

        if (currentMenuButton != null)
            Destroy(currentMenuButton);

        GameObject prefab = Resources.Load<GameObject>("MenuButton");
        if (prefab == null)
            return;

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        currentMenuButton = Instantiate(prefab, canvas.transform);
        currentMenuButton.name = "MenuButton";

        Button btn = currentMenuButton.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnMenuButtonPressed);
        }
    }

    bool IsExcluded(string sceneName)
    {
        foreach (string excluded in excludedScenes)
        {
            if (sceneName == excluded)
                return true;
        }
        return false;
    }

    public void OnMenuButtonPressed()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SetPreviousScene(currentScene);
        ScreenshotHelper.CaptureTemporaryScreenshot();
        Time.timeScale = 0f;
        SceneManager.LoadScene("LoadScene2");
    }

    public void SetPreviousScene(string sceneName)
    {
        previousScene = sceneName;
    }

    public string GetPreviousScene()
    {
        return previousScene;
    }
}

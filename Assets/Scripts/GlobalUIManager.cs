using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviour
{
    public string[] excludedScenes = { "MainMenu", "LoadScene", "AlbumScene", "SettingsScene", "PauseMenu" };
    public string pauseMenuScene = "PauseMenu";

    private static GlobalUIManager instance;
    private GameObject currentMenuButton;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        if (instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("GlobalUIManager");
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab);
                DontDestroyOnLoad(obj);
            }
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        {
            Debug.LogWarning("GlobalUIManager: MenuButton prefab not found in Resources folder.");
            return;
        }

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
        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync(pauseMenuScene, LoadSceneMode.Additive);
    }
}

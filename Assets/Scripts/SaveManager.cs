using UnityEngine;
using System.IO;
using System.Collections;

public static class SaveManager
{
    public static string GetSavePath(int slot) => Path.Combine(Application.persistentDataPath, $"save_{slot}.json");
    public static string GetPreviewPath(int slot) => Path.Combine(Application.persistentDataPath, $"preview_{slot}.png");

    public static void SaveGame(int slot)
    {
        GameObject temp = new GameObject("SaveGameRunner");
        SaveGameRunner runner = temp.AddComponent<SaveGameRunner>();
        runner.StartCoroutine(SaveGameCoroutine(slot));
    }

    public static IEnumerator SaveGameCoroutine(int slot)
    {
        SaveData data = new SaveData();
        data.sceneName = GlobalUIManager.Instance != null ? GlobalUIManager.Instance.GetPreviousScene() : UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.datetime = System.DateTime.Now.ToString("dd/MM/yy HH:mm");
        data.screenshotPath = ScreenshotHelper.CopyScreenshotToSlot(slot);
        data.dialogueLineIndex = DialogueProgressManager.ResumeLineIndex;

        yield return new WaitForSeconds(0.3f);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetSavePath(slot), json);
    }

    public static SaveData LoadGame(int slot)
    {
        string path = GetSavePath(slot);
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static bool SlotHasSave(int slot)
    {
        return File.Exists(GetSavePath(slot));
    }
}

public class SaveGameRunner : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

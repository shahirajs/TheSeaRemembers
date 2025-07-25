using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static string GetSavePath(int slot) => Path.Combine(Application.persistentDataPath, $"save_{slot}.json");
    public static string GetPreviewPath(int slot) => Path.Combine(Application.persistentDataPath, $"preview_{slot}.png");

    public static void SaveGame(int slot)
    {
        SaveData data = new SaveData();
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.datetime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        data.screenshotPath = ScreenshotHelper.CaptureScreenshot(slot);

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

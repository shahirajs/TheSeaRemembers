using UnityEngine;
using System.IO;
using System.Collections;

public static class ScreenshotHelper
{
    public static string tempPreviewPath => Path.Combine(Application.persistentDataPath, "preview_temp.png");

    public static void CaptureTemporaryScreenshot()
    {
        GameObject temp = new GameObject("ScreenshotRunner");
        ScreenshotRunner runner = temp.AddComponent<ScreenshotRunner>();
        runner.StartCoroutine(runner.Capture(tempPreviewPath));
    }

    public static string CopyScreenshotToSlot(int slot)
    {
        string destPath = Path.Combine(Application.persistentDataPath, $"preview_{slot}.png");
        if (File.Exists(tempPreviewPath))
        {
            File.Copy(tempPreviewPath, destPath, true);
        }
        return destPath;
    }

    private class ScreenshotRunner : MonoBehaviour
    {
        public IEnumerator Capture(string path)
        {
            GameObject menuButton = GameObject.Find("MenuButton");
            bool wasActive = menuButton != null && menuButton.activeSelf;

            if (menuButton != null)
                menuButton.SetActive(false);

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            int width = Screen.width;
            int height = Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            byte[] imageBytes = tex.EncodeToPNG();
            File.WriteAllBytes(path, imageBytes);

            if (menuButton != null && wasActive)
                menuButton.SetActive(true);

            Destroy(gameObject);
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

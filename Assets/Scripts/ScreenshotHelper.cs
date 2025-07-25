using System.IO;
using UnityEngine;

public static class ScreenshotHelper
{
    public static string CaptureScreenshot(int slot)
    {
        string filename = $"preview_{slot}.png";
        string path = Path.Combine(Application.persistentDataPath, filename);
        ScreenCapture.CaptureScreenshot(filename);
        return path;
    }
}

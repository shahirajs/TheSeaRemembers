using UnityEngine;
using UnityEngine.UI;

public class ThumbnailButton : MonoBehaviour
{
    public Image fullScreenTargetImage;
    public Sprite fullImage;
    public GameObject fullScreenPanel;

    public void ShowFullScreen()
    {
        fullScreenTargetImage.sprite = fullImage;
        fullScreenPanel.SetActive(true);
    }
}

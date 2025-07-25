using UnityEngine;
using UnityEngine.UI;

public class ThumbnailButton : MonoBehaviour
{
    public Image fullScreenTargetImage;
    public Sprite fullImage;
    public GameObject fullScreenPanel;
    public Button closeButton;

    public void ShowFullScreen()
    {
        if (fullScreenTargetImage != null && fullImage != null && fullScreenPanel != null)
        {
            fullScreenTargetImage.sprite = fullImage;
            fullScreenTargetImage.enabled = true;
            fullScreenPanel.SetActive(true);
        }
    }

    public void HideFullScreen()
    {
        if (fullScreenPanel != null)
            fullScreenPanel.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text buttonText;
    public Color normalColor = new Color(0.569f, 0.937f, 1f);
    public Color hoverColor = Color.white;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public float hoverVolume = 0.5f;
    public float clickVolume = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor;

        if (hoverSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(clickSound);
    }
}

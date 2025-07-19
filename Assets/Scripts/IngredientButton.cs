using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientButton : MonoBehaviour, IPointerClickHandler
{
    public string ingredientName;
    public Sprite defaultSprite;
    public Sprite selectedSprite;
    public AudioClip selectSound;
    public AudioClip deselectSound;
    public float feedbackScale = 1.2f;
    public float scaleDuration = 0.1f;

    private bool isSelected = false;
    private Image buttonImage;
    private Vector3 originalScale;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = defaultSprite;
        originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelected)
        {
            UndoSelection();
        }
        else
        {
            SelectIngredient();
        }
    }

    void SelectIngredient()
    {
        isSelected = true;
        buttonImage.sprite = selectedSprite;
        DessertCupManager.Instance.AddIngredient(ingredientName);
        PlayFeedback(selectSound);
    }

    void UndoSelection()
    {
        isSelected = false;
        buttonImage.sprite = defaultSprite;
        DessertCupManager.Instance.RemoveIngredient(ingredientName);
        PlayFeedback(deselectSound);
    }

    void PlayFeedback(AudioClip clip)
    {
        if (clip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clip);
        }
        StopAllCoroutines();
        StartCoroutine(ScaleFeedback());
    }

    System.Collections.IEnumerator ScaleFeedback()
    {
        transform.localScale = originalScale * feedbackScale;
        yield return new WaitForSeconds(scaleDuration);
        transform.localScale = originalScale;
    }

    public void ResetSelection()
    {
        isSelected = false;
        buttonImage.sprite = defaultSprite;
    }
}

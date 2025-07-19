using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientButton : MonoBehaviour, IPointerClickHandler
{
    public string ingredientName;
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    private bool isSelected = false;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = defaultSprite;
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
        //DessertCupManager.Instance.AddIngredient(ingredientName);
    }

    void UndoSelection()
    {
        isSelected = false;
        buttonImage.sprite = defaultSprite;
        //DessertCupManager.Instance.RemoveIngredient(ingredientName);
    }
}

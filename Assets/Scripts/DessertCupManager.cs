using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IngredientSprite
{
    public string ingredientName;
    public Sprite ingredientSprite;
    public Sprite checkedSprite;
}

public class DessertCupManager : MonoBehaviour
{
    public static DessertCupManager Instance;

    public Transform targetRecipePanel;
    public GameObject recipeIngredientPrefab;
    public GameObject checkButton;

    public List<string> allPossibleIngredients = new List<string>();
    public int numberOfIngredientsInRecipe = 3;
    public List<IngredientSprite> ingredientSprites = new List<IngredientSprite>();

    private Dictionary<string, Sprite> ingredientSpriteDict = new Dictionary<string, Sprite>();
    private List<string> selectedIngredients = new List<string>();
    private List<string> currentRecipe = new List<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        BuildIngredientSpriteDictionary();
        GenerateRandomRecipe();
    }

    private void BuildIngredientSpriteDictionary()
    {
        ingredientSpriteDict.Clear();
        foreach (var item in ingredientSprites)
        {
            if (!ingredientSpriteDict.ContainsKey(item.ingredientName))
                ingredientSpriteDict.Add(item.ingredientName, item.ingredientSprite);
        }
    }

    public void GenerateRandomRecipe()
    {
        if (allPossibleIngredients.Count < numberOfIngredientsInRecipe)
            return;

        currentRecipe.Clear();
        selectedIngredients.Clear();

        List<string> tempPool = new List<string>(allPossibleIngredients);

        for (int i = 0; i < numberOfIngredientsInRecipe; i++)
        {
            int index = Random.Range(0, tempPool.Count);
            currentRecipe.Add(tempPool[index]);
            tempPool.RemoveAt(index);
        }

        BuildTargetRecipeVisual();
        checkButton.SetActive(false);
    }

    private void BuildTargetRecipeVisual()
    {
        foreach (Transform child in targetRecipePanel)
            Destroy(child.gameObject);

        foreach (string ingredientName in currentRecipe)
        {
            GameObject icon = Instantiate(recipeIngredientPrefab, targetRecipePanel);
            icon.name = ingredientName;

            Image mainImage = icon.GetComponent<Image>();
            mainImage.sprite = GetIngredientSprite(ingredientName);
        }
    }

    public void AddIngredient(string ingredientName)
    {
        if (selectedIngredients.Contains(ingredientName)) return;

        selectedIngredients.Add(ingredientName);
        MarkIngredientInTarget(ingredientName);

        if (selectedIngredients.Count == currentRecipe.Count)
            checkButton.SetActive(true);
    }

    public void RemoveIngredient(string ingredientName)
    {
        if (!selectedIngredients.Contains(ingredientName)) return;

        selectedIngredients.Remove(ingredientName);
        UnmarkIngredientInTarget(ingredientName);
        checkButton.SetActive(false);
    }

    private void MarkIngredientInTarget(string ingredientName)
    {
        foreach (Transform item in targetRecipePanel)
        {
            if (item.name == ingredientName)
            {
                Image mainImage = item.GetComponent<Image>();
                mainImage.sprite = GetCheckedSprite(ingredientName);
            }
        }
    }

    private void UnmarkIngredientInTarget(string ingredientName)
    {
        foreach (Transform item in targetRecipePanel)
        {
            if (item.name == ingredientName)
            {
                Image mainImage = item.GetComponent<Image>();
                mainImage.sprite = GetIngredientSprite(ingredientName);
            }
        }
    }

    public List<string> GetSelectedIngredients()
    {
        return selectedIngredients;
    }

    public List<string> GetCurrentRecipe()
    {
        return currentRecipe;
    }

    private Sprite GetIngredientSprite(string ingredientName)
    {
        if (ingredientSpriteDict.ContainsKey(ingredientName))
            return ingredientSpriteDict[ingredientName];
        return null;
    }

    private Sprite GetCheckedSprite(string ingredientName)
    {
        foreach (var item in ingredientSprites)
        {
            if (item.ingredientName == ingredientName)
                return item.checkedSprite;
        }
        return null;
    }

    public void ResetAllIngredientButtons()
    {
        IngredientButton[] buttons = FindObjectsOfType<IngredientButton>();
        foreach (IngredientButton button in buttons)
        {
            button.ResetSelection();
        }
    }
}

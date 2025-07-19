using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DessertGameManager : MonoBehaviour
{
    public float timeLimit = 30f;
    public float resetDelay = 2f;
    public float timesUpDisplayDuration = 1f;
    public float successDelay = 1f;
    public string successSceneName = "DessertShowcase";
    public Image timerImage;
    public GameObject timesUpImage;
    public Sprite[] numberSprites;

    public AudioClip tickSound;
    public AudioClip clickSound;

    private float currentTime;
    private bool isGameActive = false;
    private bool hasCompleted = false;
    private int lastSecond = -1;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!isGameActive || hasCompleted)
            return;

        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (currentTime <= 0)
        {
            isGameActive = false;
            StartCoroutine(HandleTimeOutSequence());
        }
    }

    void StartTimer()
    {
        currentTime = timeLimit;
        isGameActive = true;
        hasCompleted = false;
        lastSecond = -1;
        UpdateTimerDisplay();
        timesUpImage.SetActive(false);
    }

    void UpdateTimerDisplay()
    {
        int timeRemaining = Mathf.Clamp(Mathf.CeilToInt(currentTime), 0, numberSprites.Length - 1);
        timerImage.sprite = numberSprites[timeRemaining];

        if (timeRemaining != lastSecond)
        {
            PlayTickSound();
            lastSecond = timeRemaining;
        }
    }

    public void OnCheckButtonPressed()
    {
        if (!isGameActive || hasCompleted)
            return;

        PlayClickSound();

        var selected = DessertCupManager.Instance.GetSelectedIngredients();
        var target = DessertCupManager.Instance.GetCurrentRecipe();

        if (IsSelectionCorrect(selected, target))
        {
            hasCompleted = true;
            isGameActive = false;
            StartCoroutine(HandleSuccess());
        }
    }

    bool IsSelectionCorrect(System.Collections.Generic.List<string> selected, System.Collections.Generic.List<string> target)
    {
        if (selected.Count != target.Count)
            return false;

        for (int i = 0; i < selected.Count; i++)
        {
            if (selected[i] != target[i])
                return false;
        }
        return true;
    }

    IEnumerator HandleSuccess()
    {
        yield return new WaitForSeconds(successDelay);
        SceneManager.LoadScene(successSceneName);
    }

    IEnumerator HandleTimeOutSequence()
    {
        timesUpImage.SetActive(true);
        yield return new WaitForSeconds(timesUpDisplayDuration);
        timesUpImage.SetActive(false);
        yield return new WaitForSeconds(resetDelay);
        DessertCupManager.Instance.ResetAllIngredientButtons();
        DessertCupManager.Instance.GenerateRandomRecipe();
        StartTimer();
    }

    void PlayTickSound()
    {
        if (tickSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(tickSound);
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clickSound);
        }
    }
}

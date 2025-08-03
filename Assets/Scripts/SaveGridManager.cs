using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class SaveGridManager : MonoBehaviour
{
    public GameObject thumbnailPrefab;
    public Transform thumbnailGrid;
    public GameObject pageButtonPrefab;
    public Transform pageButtonContainer;
    public Button prevPageButton;
    public Button nextPageButton;
    public int totalSlots = 20;
    public int slotsPerPage = 4;
    public SettingsUIManager settingsUIManager;

    private int currentPage = 0;
    private List<Button> pageButtons = new List<Button>();

    void Start()
    {
        GeneratePageButtons();
        ShowPage(0);
        prevPageButton.onClick.AddListener(() => ShowPage(currentPage - 1));
        nextPageButton.onClick.AddListener(() => ShowPage(currentPage + 1));
    }

    void GeneratePageButtons()
    {
        int pageCount = Mathf.CeilToInt((float)totalSlots / slotsPerPage);

        foreach (Transform child in pageButtonContainer)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null)
                btn.onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }

        pageButtons.Clear();

        for (int i = 0; i < pageCount; i++)
        {
            GameObject btnObj = Instantiate(pageButtonPrefab, pageButtonContainer);
            btnObj.SetActive(true);

            Button btn = btnObj.GetComponent<Button>();
            btn.enabled = true;

            int index = i;
            btn.onClick.AddListener(() => ShowPage(index));

            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>(true);
            if (txt != null)
            {
                txt.text = (i + 1).ToString();
                txt.gameObject.SetActive(true);
                txt.enabled = true;
            }

            ButtonTextColorChange colorChange = btn.GetComponent<ButtonTextColorChange>();
            if (colorChange != null)
            {
                colorChange.buttonText = txt;
                colorChange.isActivePage = (i == currentPage);
                colorChange.UpdateColor();
            }

            pageButtons.Add(btn);
        }
    }

    void ShowPage(int pageIndex)
    {
        int maxPage = Mathf.CeilToInt((float)totalSlots / slotsPerPage) - 1;
        if (pageIndex < 0 || pageIndex > maxPage)
            return;

        currentPage = pageIndex;

        foreach (Transform child in thumbnailGrid)
            Destroy(child.gameObject);

        int start = pageIndex * slotsPerPage;
        int end = Mathf.Min(start + slotsPerPage, totalSlots);

        for (int i = start; i < end; i++)
        {
            int slotIndex = i;
            GameObject thumb = Instantiate(thumbnailPrefab, thumbnailGrid);

            TMP_Text dateText = thumb.transform.Find("dateLabel")?.GetComponent<TMP_Text>();
            Image thumbnailImage = thumb.transform.Find("screenshot")?.GetComponent<Image>();
            Button button = thumb.GetComponent<Button>();

            UpdateThumbnail(slotIndex, dateText, thumbnailImage);

            if (button != null)
            {
                int index = slotIndex;
                TMP_Text text = dateText;
                Image img = thumbnailImage;

                button.onClick.AddListener(() => HandleSlotClick(index, text, img));
            }
        }

        for (int i = 0; i < pageButtons.Count; i++)
        {
            ButtonTextColorChange colorChange = pageButtons[i].GetComponent<ButtonTextColorChange>();
            if (colorChange != null)
            {
                colorChange.isActivePage = (i == pageIndex);
                colorChange.UpdateColor();
            }
        }

        prevPageButton.gameObject.SetActive(currentPage > 0);
        nextPageButton.gameObject.SetActive(currentPage < maxPage);
    }

    void HandleSlotClick(int slotIndex, TMP_Text dateText, Image thumbnailImage)
    {
        if (SaveManager.SlotHasSave(slotIndex))
        {
            if (settingsUIManager != null)
            {
                settingsUIManager.ShowConfirmation("Overwrite this save slot?", () =>
                {
                    StartCoroutine(DelayedThumbnailUpdate(slotIndex, dateText, thumbnailImage));
                });
            }
        }
        else
        {
            StartCoroutine(DelayedThumbnailUpdate(slotIndex, dateText, thumbnailImage));
        }
    }

    IEnumerator DelayedThumbnailUpdate(int slotIndex, TMP_Text dateText, Image thumbnailImage)
    {
        yield return new WaitForSeconds(0.3f);

        string copiedPath = ScreenshotHelper.CopyScreenshotToSlot(slotIndex);

        SaveData data = new SaveData();
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.datetime = System.DateTime.Now.ToString("dd/MM/yy HH:mm");
        data.screenshotPath = copiedPath;
        data.dialogueLineIndex = DialogueProgressManager.ResumeLineIndex;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SaveManager.GetSavePath(slotIndex), json);

        UpdateThumbnail(slotIndex, dateText, thumbnailImage);
    }

    void UpdateThumbnail(int slotIndex, TMP_Text dateText, Image thumbnailImage)
    {
        if (SaveManager.SlotHasSave(slotIndex))
        {
            SaveData data = SaveManager.LoadGame(slotIndex);
            if (dateText != null)
                dateText.text = data.datetime;

            if (thumbnailImage != null && File.Exists(data.screenshotPath))
            {
                byte[] imageBytes = File.ReadAllBytes(data.screenshotPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);

                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(texture, rect, pivot);

                thumbnailImage.sprite = sprite;
                thumbnailImage.preserveAspect = true;
                thumbnailImage.SetNativeSize();
            }
        }
        else
        {
            if (dateText != null)
                dateText.text = "Empty";

            if (thumbnailImage != null)
                thumbnailImage.sprite = null;
        }
    }
}

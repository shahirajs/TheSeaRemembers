using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public GameObject thumbnailPrefab;
    public Transform thumbnailGrid;
    public GameObject pageButtonPrefab;
    public Transform pageButtonContainer;
    public Button prevPageButton;
    public Button nextPageButton;

    private int currentPage = 0;
    private int slotsPerPage = 4;
    private int totalSlots = 20;
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
            Destroy(child.gameObject);

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
            Button button = thumb.GetComponent<Button>();

            if (SaveManager.SlotHasSave(slotIndex))
            {
                SaveData data = SaveManager.LoadGame(slotIndex);
                if (dateText != null)
                    dateText.text = data.datetime;

                button.onClick.AddListener(() =>
                {
                    PlayerPrefs.SetString("LoadFromSlot", slotIndex.ToString());
                    SceneManager.LoadScene(data.sceneName);
                });
            }
            else
            {
                if (dateText != null)
                    dateText.text = "Empty";
                button.interactable = false;
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
}

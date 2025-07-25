using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AlbumManager : MonoBehaviour
{
    public List<Sprite> images;
    public GameObject thumbnailPrefab;
    public Transform thumbnailGrid;
    public GameObject fullscreenPanel;
    public Image fullscreenImage;

    public GameObject pageButtonPrefab;
    public Transform pageButtonContainer;
    public Button prevPageButton;
    public Button nextPageButton;

    private int currentPage = 0;
    private int imagesPerPage = 4;
    private List<Button> pageButtons = new List<Button>();

    void Start()
    {
        if (images == null || images.Count == 0)
        {
            prevPageButton.interactable = false;
            nextPageButton.interactable = false;
            return;
        }

        if (thumbnailPrefab == null)
        {
            Debug.LogError("Thumbnail prefab not assigned.");
            return;
        }

        GeneratePageButtons();
        ShowPage(0);
        prevPageButton.onClick.AddListener(() => ShowPage(currentPage - 1));
        nextPageButton.onClick.AddListener(() => ShowPage(currentPage + 1));
    }

    void GeneratePageButtons()
    {
        int pageCount = Mathf.CeilToInt((float)images.Count / imagesPerPage);

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

            pageButtons.Add(btn);
        }
    }

    void ShowPage(int pageIndex)
    {
        int maxPage = Mathf.CeilToInt((float)images.Count / imagesPerPage) - 1;
        if (pageIndex < 0 || pageIndex > maxPage)
            return;

        currentPage = pageIndex;

        foreach (Transform child in thumbnailGrid)
        {
            Destroy(child.gameObject);
        }

        if (thumbnailPrefab == null)
        {
            Debug.LogError("Thumbnail prefab is null. Cannot create thumbnails.");
            return;
        }

        int start = pageIndex * imagesPerPage;
        int end = Mathf.Min(start + imagesPerPage, images.Count);

        for (int i = start; i < end; i++)
        {
            GameObject thumb = Instantiate(thumbnailPrefab, thumbnailGrid);
            Image img = thumb.GetComponentInChildren<Image>();
            img.enabled = true;
            img.sprite = images[i];

            ThumbnailButton tb = thumb.GetComponent<ThumbnailButton>();
            tb.fullImage = images[i];
            tb.fullScreenTargetImage = fullscreenImage;
            tb.fullScreenPanel = fullscreenPanel;

            thumb.GetComponent<Button>().onClick.AddListener(tb.ShowFullScreen);
        }

        for (int i = 0; i < pageButtons.Count; i++)
        {
            ColorBlock cb = pageButtons[i].colors;
            cb.normalColor = (i == pageIndex) ? Color.cyan : Color.white;
            pageButtons[i].colors = cb;
        }

        prevPageButton.interactable = (currentPage > 0);
        nextPageButton.interactable = (currentPage < maxPage);
    }
}

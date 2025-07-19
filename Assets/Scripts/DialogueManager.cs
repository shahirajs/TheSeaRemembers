using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea]
    public string text;
}

public class DialogueManager : MonoBehaviour
{
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public DialogueLine[] dialogueLines;
    public float textSpeed = 0.05f;
    public AudioClip typingSound;
    public int charactersPerSound = 2;

    public AudioClip bellSound;
    public GameObject characterImage;
    public GameObject textBox;
    public float characterAppearDelay = 2f;
    public float textBoxAppearDelay = 1f;
    public float characterHideDelay = 1f;
    public float textBoxHideDelay = 0.5f;
    public float bellToCharacterDelay = 1f;

    private int currentLineIndex;
    private bool isTyping;
    private string currentText;
    private bool canProceed = false;
    private bool isDialogueRunning = false;
    public event System.Action OnDialogueComplete;

    public int dessertRequestLineIndex = 1;

    void Start()
    {
        if (DialogueProgressManager.ResumeLineIndex > 0)
        {
            currentLineIndex = DialogueProgressManager.ResumeLineIndex;
            ShowLine();
            canProceed = true;
        }
        else
        {
            StartDialogue();
        }
    }

    void Update()
    {
        if (!canProceed) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentText;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        isDialogueRunning = true;
        canProceed = false;
        characterImage.SetActive(false);
        textBox.SetActive(false);
        speakerNameText.text = "";
        dialogueText.text = "";
        StartCoroutine(StartDialogueSequence());
    }

    IEnumerator StartDialogueSequence()
    {
        speakerNameText.text = "";
        dialogueText.text = "";

        yield return new WaitForSeconds(characterAppearDelay);
        PlayBellSound();
        yield return new WaitForSeconds(bellToCharacterDelay);
        characterImage.SetActive(true);

        yield return new WaitForSeconds(textBoxAppearDelay);
        textBox.SetActive(true);

        currentLineIndex = currentLineIndex == 0 ? 0 : currentLineIndex;
        ShowLine();

        canProceed = true;
    }

    void ShowLine()
    {
        DialogueLine line = dialogueLines[currentLineIndex];
        speakerNameText.text = line.speakerName;
        currentText = line.text;
        dialogueText.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        int charCount = 0;

        foreach (char letter in currentText.ToCharArray())
        {
            dialogueText.text += letter;
            charCount++;

            if (typingSound != null && charCount % charactersPerSound == 0 && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(typingSound);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            if (currentLineIndex == dessertRequestLineIndex)
            {
                DialogueProgressManager.ResumeLineIndex = currentLineIndex;
                DialogueProgressManager.LastScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("DessertGame");
                return;
            }
            ShowLine();
        }
        else
        {
            StartCoroutine(EndDialogueSequence());
        }
    }

    IEnumerator EndDialogueSequence()
    {
        canProceed = false;

        yield return new WaitForSeconds(textBoxHideDelay);
        textBox.SetActive(false);
        dialogueText.text = "";
        speakerNameText.text = "";

        yield return new WaitForSeconds(characterHideDelay);
        characterImage.SetActive(false);
        yield return new WaitForSeconds(bellToCharacterDelay);
        PlayBellSound();

        isDialogueRunning = false;

        if (OnDialogueComplete != null)
        {
            OnDialogueComplete.Invoke();
        }
    }

    void PlayBellSound()
    {
        if (bellSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(bellSound);
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueRunning;
    }
}

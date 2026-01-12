using UnityEngine;
using TMPro;
using System.Collections;


[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(2, 5)] public string text;
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private DialogueLine[] dialogueLines;

    private Coroutine typingCoroutine;
    private int currentLineIndex;
    private bool isDialogueActive;

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            if (typingCoroutine != null)
                SkipTyping();
            else
                ShowNextLine();
        }
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        dialogueLines = lines;
        currentLineIndex = 0;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);
        ShowLine(currentLineIndex);
    }

    private void ShowLine(int index)
    {
        if (index >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        var line = dialogueLines[index];

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null;
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueLines[currentLineIndex].text;
            typingCoroutine = null;
        }
    }

    private void ShowNextLine()
    {
        currentLineIndex++;
        ShowLine(currentLineIndex);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }

    public void StartPredefinedDialogue()
    {
        StartDialogue(dialogueLines);
    }
}

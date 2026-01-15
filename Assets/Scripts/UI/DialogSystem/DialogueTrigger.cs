using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData[] dialogueData;
    [SerializeField] private DialogueSystem dialogueSystem;

    private int _currentDialogueIndex = 1;

    private void OnEnable()
    {
        dialogueSystem.StartDialogue(dialogueData[0].lines);
    }

    public void StartNextDialogue()
    {
        print(_currentDialogueIndex);
        dialogueSystem.StartDialogue(dialogueData[_currentDialogueIndex].lines);
        _currentDialogueIndex += 1;
    }
}

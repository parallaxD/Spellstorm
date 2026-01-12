using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private DialogueSystem dialogueSystem;

    private void Start()
    {
        dialogueSystem.StartDialogue(dialogueData.lines);
    }
}

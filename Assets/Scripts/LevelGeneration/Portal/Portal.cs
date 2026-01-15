using System.Collections.Generic;
using UnityEngine;
using static LevelGenerationConstants;

public class Portal : MonoBehaviour
{
    [SerializeField] GenerationManager generationManager;
    [SerializeField] private DialogueTrigger dialogueTrigger;

    private Vector2 startPosition = new Vector2(13.5f, 1f);

    public static List<Vector2> roomStartPositions;

    public void Awake()
    {
        ResetPosition();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            generationManager.GenerateNextLocation();
            dialogueTrigger.StartNextDialogue();
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }

    public void UpdatePosition()
    {
        var lastRoomPosition = roomStartPositions[roomStartPositions.Count - 1];
        transform.position = lastRoomPosition;
    }
}
using UnityEngine;

public class InteractionArea : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (collision.TryGetComponent<IInteractable>(out IInteractable interactableObject))
            {
                interactableObject.Interaction();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactableObject))
        {
            interactableObject.ShowInteractionText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactableObject))
        {
            interactableObject.HideInteractionText();
        }
    }
}

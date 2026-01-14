using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject progressionUI;

    public void Interaction()
    {
        if (!progressionUI.activeSelf)
        {
            progressionUI.SetActive(true);
        }
        else progressionUI.SetActive(false);
    }

    void IInteractable.ShowInteractionText()
    {
        interactionText.SetActive(true);
    }

    void IInteractable.HideInteractionText()
    {
        interactionText.SetActive(false);
    }
}

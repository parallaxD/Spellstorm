using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public Image highlightImage;

    private CanvasGroup highlightCanvasGroup;

    void Awake()
    {
        highlightCanvasGroup = highlightImage.GetComponent<CanvasGroup>();
        highlightCanvasGroup.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightCanvasGroup == null)
        {
            highlightImage.gameObject.SetActive(true);
        }
        else
        {
            highlightCanvasGroup.alpha = 1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightCanvasGroup == null)
        {
            highlightImage.gameObject.SetActive(false);
        }
        else
        {
            highlightCanvasGroup.alpha = 0f;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlightImage;

    void Awake()
    {
        highlightImage.SetActive(false); 
    }

    public void OnPointerEnter(PointerEventData eventData) => highlightImage.SetActive(true);

    public void OnPointerExit(PointerEventData eventData) => highlightImage.SetActive(false);

    void OnDisable()
    {
        if (highlightImage != null) highlightImage.SetActive(false);
    }
}

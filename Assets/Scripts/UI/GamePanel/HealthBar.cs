using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    private RectTransform _barRectTransform;
    private float _maxWidth;

    private void Awake()
    {
        _barRectTransform = barImage.GetComponent<RectTransform>();
        _maxWidth = _barRectTransform.sizeDelta.x;
    }

    public void UpdateBar(float percentage)
    {
        var newWidth = _maxWidth * percentage;

        _barRectTransform.sizeDelta = new Vector2(newWidth, _barRectTransform.sizeDelta.y);
    }
}
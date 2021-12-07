using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Style text and add subscriber
/// </summary>
[RequireComponent(typeof(Button))]
public class BtnStyler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color _normalTextColor;
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;

    void OnEnable()
    {
        _button.onClick.AddListener(Clicked);
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(Clicked);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _text.color = _selectedTextColor;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _text.color = _normalTextColor;
    }

    void Clicked()
    {
        _text.color = _normalTextColor;
    }

}

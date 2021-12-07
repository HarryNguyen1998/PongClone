using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderTextUpdate : MonoBehaviour
{
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private Slider _slider;

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(UpdateText);
        _valueText.text = _slider.value.ToString("0.00");
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(UpdateText);
    }

    void UpdateText(float value)
    {
        _valueText.text = value.ToString("0.00");
    }
}

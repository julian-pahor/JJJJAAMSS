using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ValueEditor : MonoBehaviour
{
    //prefab component that lets us edit a float/int with sliders n textfields

    public Slider slider;
    public TextMeshProUGUI displayUI;
    public TMP_InputField inputField;

    string displayText;
    float currentValue;


    private void Start()
    {
        inputField.onEndEdit.AddListener(delegate { OnEditInput(); });
        slider.onValueChanged.AddListener(delegate { OnEditSlider(); });
    }

    void OnEditSlider()
    {
        currentValue = slider.value;
        Refresh(currentValue);
    }

    void OnEditInput()
    {
        float parsed;
        if (float.TryParse(inputField.text, out parsed))
        {
            Refresh(parsed);
        }
    }

    void Refresh(float newValue)
    {
        currentValue = newValue;
        inputField.text = currentValue.ToString("0.00");
        slider.value = currentValue;
      
    }
}

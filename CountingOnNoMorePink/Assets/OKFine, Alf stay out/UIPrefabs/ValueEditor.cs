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
    public InputField inputField;

    string displayText;
    float currentValue;

   

    void Refresh(float value)
    {
        inputField.text = value.ToString("0.00");
        slider.value = value;
    }
}

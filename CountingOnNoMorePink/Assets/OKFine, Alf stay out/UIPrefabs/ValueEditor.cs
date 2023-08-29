using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ValueEditor : MonoBehaviour
{
    //prefab component that lets us edit a float/int with sliders n textfields

    public Slider slider;
    public TextMeshProUGUI displayUI;
    public TMP_InputField inputField;

    float currentValue;

    public Action<float> onChange;

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

        if(onChange != null)
        {
            onChange(currentValue);
        }
      
    }

    /// <summary>
    /// Can potentially change this to use UnityAction as well to subscribe directly
    /// to the event calls on UI objects
    /// </summary>
    /// <param name="func"></param>
    /// <param name="f"></param>
    /// <param name="s"></param>
    public void SetListener(Action<float> func, float f, string s)
    {
        Refresh(f);
        displayUI.text = s;
        onChange = func;
    }

    public void RemoveListeners()
    {
        inputField.onEndEdit.RemoveAllListeners();
        slider.onValueChanged.RemoveAllListeners();
    }
}

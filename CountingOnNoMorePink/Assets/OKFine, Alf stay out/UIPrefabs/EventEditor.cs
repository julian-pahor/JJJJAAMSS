using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventEditor : MonoBehaviour
{
    public AttackEvent currentlySelectedEvent;

    public TextMeshProUGUI selectedEventName;


    //make prefab thinfs
    public Slider shotsSlider;
    public TextMeshProUGUI shotsSliderText;

    public Slider offsetSlider;
    public TextMeshProUGUI offsetSliderText;

    //Prefab To use for all value editing
    public ValueEditor editorPreFab;
    public Transform editorParent;

    public List<ValueEditor> currentEditors = new List<ValueEditor>();
 

    private void Start()
    {
       
    }

    public void SelectNewObject(AttackEvent attackEvent)
    {
        //Unhook up and destroy currently used UI
        foreach(ValueEditor editor in currentEditors)
        {
            editor.RemoveListeners();
            Destroy(editor);
        }

        currentlySelectedEvent = attackEvent;

        //set all our values and display name to match the selected object
        if(attackEvent == null)
        {
            selectedEventName.text = "No event selected";
            return;
        }

        selectedEventName.text = currentlySelectedEvent.name;

        attackEvent.HookUp(this);

        RefreshValues();
    }

    public ValueEditor CreateEditor()
    {
        ValueEditor ve = Instantiate(editorPreFab, editorParent);

        return ve;
    }

    //call when we select something to change our sliders to match its data
    void RefreshValues()
    {
        if(currentlySelectedEvent == null)
            return;
        
        switch(currentlySelectedEvent)
        {
            case SpreadShot ss:

                shotsSlider.value = ss.shots;
                shotsSliderText.text = "Shots: " + shotsSlider.value;

                offsetSlider.value = ss.arcOffset;
                offsetSliderText.text = "Offset " + offsetSlider.value.ToString("00.0");

                break;

            case BeamShot bs:
                shotsSlider.value = bs.beams;
                shotsSliderText.text = "Beams: " + shotsSlider.value;

                offsetSlider.value = bs.arcOffset;
                offsetSliderText.text = "Offset " + offsetSlider.value.ToString("00.0");

                break;


        }

    }
    //called when we change a slider to update the target event
    public void RefreshValuesOnChange()
    {

        if (currentlySelectedEvent == null)
            return;

        switch (currentlySelectedEvent)
        {
            case SpreadShot ss:

                ss.shots = (int)shotsSlider.value;
                ss.arcOffset = offsetSlider.value;



                break;

            case BeamShot bs:
                bs.beams = (int)shotsSlider.value;
                bs.arcOffset = offsetSlider.value;
                break;

        }

        RefreshValues();

    }


}

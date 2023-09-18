using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventEditor : MonoBehaviour
{
    public AttackEvent currentlySelectedEvent;

    public TMP_InputField selectedEventName;

    public PrefabEventList prefabsList;

    //Prefab To use for all value editing
    public ValueEditor editorPreFab;
    public Transform editorParent;

    public List<ValueEditor> currentEditors = new List<ValueEditor>();

    private void Start()
    {
       
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {

            SaveSelectedAsPrefab();
        }
    }

    void SaveSelectedAsPrefab()
    {
        if(currentlySelectedEvent != null)
        {
            prefabsList.AddPrefabEvent(currentlySelectedEvent);
            prefabsList.SaveAll();
        }
    }

    public void OnNameTextChanged()
    {
        if(currentlySelectedEvent == null) return;

        currentlySelectedEvent.displayName = selectedEventName.text;
    }

    public void SelectNewObject(AttackEvent attackEvent)
    {

        //Unhook up and destroy currently used UI
        foreach(ValueEditor editor in currentEditors)
        {
            editor.RemoveListeners();
            Destroy(editor.gameObject);
        }

        currentEditors.Clear();

        currentlySelectedEvent = attackEvent;

        //set all our values and display name to match the selected object
        if(attackEvent == null)
        {
            selectedEventName.text = "No event selected";
            return;
        }

        selectedEventName.text = currentlySelectedEvent.displayName;

        //Call Attackevent to create editors as neccesary
        attackEvent.HookUp(this);

        //RefreshValues();
    }

    public ValueEditor CreateEditor()
    {
        ValueEditor ve = Instantiate(editorPreFab, editorParent);
        currentEditors.Add(ve);

        return ve;
    }

    //call when we select something to change our sliders to match its data
    //void RefreshValues()
    //{
    //    if(currentlySelectedEvent == null)
    //        return;
        
    //    switch(currentlySelectedEvent)
    //    {
    //        case SpreadShot ss:

    //            shotsSlider.value = ss.shots;
    //            shotsSliderText.text = "Shots: " + shotsSlider.value;

    //            offsetSlider.value = ss.arcOffset;
    //            offsetSliderText.text = "Offset " + offsetSlider.value.ToString("00.0");

    //            break;

    //        case BeamShot bs:
    //            shotsSlider.value = bs.beams;
    //            shotsSliderText.text = "Beams: " + shotsSlider.value;

    //            offsetSlider.value = bs.arcOffset;
    //            offsetSliderText.text = "Offset " + offsetSlider.value.ToString("00.0");

    //            break;


    //    }

    //}
    ////called when we change a slider to update the target event
    //public void RefreshValuesOnChange()
    //{

    //    if (currentlySelectedEvent == null)
    //        return;

    //    switch (currentlySelectedEvent)
    //    {
    //        case SpreadShot ss:

    //            ss.shots = (int)shotsSlider.value;
    //            ss.arcOffset = offsetSlider.value;



    //            break;

    //        case BeamShot bs:
    //            bs.beams = (int)shotsSlider.value;
    //            bs.arcOffset = offsetSlider.value;
    //            break;

    //    }

    //    RefreshValues();

    //}


}

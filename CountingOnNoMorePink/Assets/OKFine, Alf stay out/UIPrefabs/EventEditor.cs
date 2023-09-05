using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventEditor : MonoBehaviour
{
    public AttackEvent currentlySelectedEvent;

    public TMP_InputField selectedEventName;

    //Prefab To use for all value editing
    public ValueEditor editorPreFab;
    public Transform editorParent;

    public List<ValueEditor> currentEditors = new List<ValueEditor>();

    private void Start()
    {
       
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

     
    }

    public ValueEditor CreateEditor()
    {
        ValueEditor ve = Instantiate(editorPreFab, editorParent);
        currentEditors.Add(ve);

        return ve;
    }


}

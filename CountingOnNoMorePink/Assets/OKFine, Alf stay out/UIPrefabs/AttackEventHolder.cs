using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AttackEventHolder : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public BeatItem preFab;
    public AttackEvent attackEvent;
    public TextMeshProUGUI text;

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeatItem slot = Instantiate(preFab, eventData.position, Quaternion.identity);
        slot.SetAttack(attackEvent);
        slot.DragInitialise(transform.root);
        eventData.pointerDrag = slot.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) //"Also equal to 1... IT'S AN ENUM" ~ Alf
        {
            //Pop open preview + editing window
        }
    }

    public void SetEvent(AttackEvent ae)
    {
        attackEvent = ae;
        text.text = attackEvent.name;
    }
}

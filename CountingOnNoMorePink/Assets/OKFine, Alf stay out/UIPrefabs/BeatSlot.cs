using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BeatSlot : MonoBehaviour, IDropHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler
{
    //maybe put the prefab in the timeline editor and get it from there
    public BeatItem preFab;
    public AttackEventUICard uiCard;

    AttackEvent attackEvent; // If attackEvent is null that means this event should be dictated by a rest

    private void Start()
    {
        UpdateSlot();
    }

    //--helper functions--
    public AttackEvent GetSlotEvent()
    {
        return attackEvent;
    }
    public void SetSlotEvent(AttackEvent attackEvent)
    {
        this.attackEvent = attackEvent;
   
        UpdateSlot();
    }
    //----------------------

    //dropping a beat item on a slots sets our attack event to the one stored in the beat item
    public void OnDrop(PointerEventData eventData)
    {
       
        GameObject drop = eventData.pointerDrag;
        BeatItem draggedItem = drop.GetComponent<BeatItem>();

        if (draggedItem != null)
        {
            SetSlotEvent(draggedItem.thisEvent);
        }

    }

    
    //right click to clear slot
    public void OnPointerUp(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Right)
        {
            this.attackEvent = null;
            UpdateSlot();
            
        }
    }

    //sets our uiCard to reflect the state of the slot
    public void UpdateSlot()
    {
        uiCard.SetDisplay(attackEvent);
    }

    //create a dummy object to allow draggin between slots
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (attackEvent == null)
            return;

        BeatItem item = Instantiate(preFab, eventData.position, Quaternion.identity);
        item.SetAttack(attackEvent);
        item.DragInitialise(transform.root);
        eventData.pointerDrag = item.gameObject;

        attackEvent = null;
        UpdateSlot();
    }

    //need these interfaces or the other ones don't work cheers thx unity
    public void OnPointerDown(PointerEventData eventData)
    {
       
    }   
    
    public void OnDrag(PointerEventData eventData)
    {
       
    }
}
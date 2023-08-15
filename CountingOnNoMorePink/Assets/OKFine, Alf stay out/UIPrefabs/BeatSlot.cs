using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatSlot : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    // If BeatItem is null that means this event should be dictated by a rest
   // [HideInInspector] public BeatItem thisItem;

    public AttackEventUICard uiCard;
    AttackEvent attackEvent;

    private void Start()
    {
        
        UpdateSlot();
    }

    public void OnDrop(PointerEventData eventData)
    {
       
            GameObject drop = eventData.pointerDrag;
            BeatItem draggedItem = drop.GetComponent<BeatItem>();

        if (draggedItem != null)
        {
            SetSlotEvent(draggedItem.thisEvent);

            Destroy(draggedItem.gameObject);
          
        }

    }

    public AttackEvent GetSlotEvent()
    {
        return attackEvent;
    }
    public void SetSlotEvent(AttackEvent attackEvent)
    {
        this.attackEvent = attackEvent;
   
        UpdateSlot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Right)
        {
            this.attackEvent = null;
            UpdateSlot();
            
        }
    }

    public void UpdateSlot()
    {
        Debug.Log("boing");
        uiCard.SetDisplay(attackEvent);
    }
}

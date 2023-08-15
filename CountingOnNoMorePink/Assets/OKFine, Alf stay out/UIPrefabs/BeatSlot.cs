using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatSlot : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    // If BeatItem is null that means this event should be dictated by a rest
    [HideInInspector] public BeatItem thisItem;

    private void Start()
    {
        thisItem = this.GetComponentInChildren<BeatItem>();
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
            UpdateSlot();
        }

    }

    public AttackEvent GetSlotEvent()
    {
        if (thisItem != null)
            return thisItem.thisEvent;
        else return null;
    }
    public void SetSlotEvent(AttackEvent attackEvent)
    {
        if (thisItem == null) return;
        thisItem.thisEvent = attackEvent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Right)
        {
            thisItem.thisEvent = null;
            UpdateSlot();
        }
    }

    public void UpdateSlot()
    {
        if (thisItem.thisEvent == null)
        {
            thisItem.gameObject.SetActive(false);
        }
        else
        {
            thisItem.SetAttack(thisItem.thisEvent);
            thisItem.gameObject.SetActive(true);
        }
    }
}

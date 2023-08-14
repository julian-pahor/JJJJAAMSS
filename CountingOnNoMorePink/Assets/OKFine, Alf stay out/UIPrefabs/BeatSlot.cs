using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(this.transform.childCount == 0)
        {
            GameObject drop = eventData.pointerDrag;
            DraggableItem draggedItem = drop.GetComponent<DraggableItem>();
            draggedItem.parentAfterDrag = transform;
        }
        
    }

    
}

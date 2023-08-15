using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatSlot : MonoBehaviour, IDropHandler
{
    // If BeatItem is null that means this event should be dictated by a rest
    [HideInInspector] public BeatItem thisItem;
    public void OnDrop(PointerEventData eventData)
    {
        if(this.transform.childCount == 0)
        {
            GameObject drop = eventData.pointerDrag;
            BeatItem draggedItem = drop.GetComponent<BeatItem>();
            draggedItem.parentAfterDrag = transform;
        }
    }
}

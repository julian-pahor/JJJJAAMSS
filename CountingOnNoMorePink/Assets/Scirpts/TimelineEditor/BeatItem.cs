using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeatItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //The rework of the timelineEditor makes this object a dummy:
    //it holds a reference to the attackEvent for passing between slots
    //it should always be destroyed after releasing it

    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public AttackEvent thisEvent;
    public TextMeshProUGUI text;

    public void DragInitialise(Transform root)
    {
        transform.SetParent(root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent;
        DragInitialise(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        //If an object is created and not dragged onto a beatslot it is destroyed
        if (transform.parent == null)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetAttack(AttackEvent ae)
    {
        thisEvent = ae;
        text.text = ae.displayName;
    }
}

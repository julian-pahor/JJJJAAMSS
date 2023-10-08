using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AttackEventHolder : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public BeatItem preFab;
    public AttackEvent attackEvent;
    public TextMeshProUGUI text;
    public GameObject deleteButton;

    PrefabEventList prefabEventList;

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

    ///we edit events directly from the timline, so this is obsolete
    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) //"Also equal to 1... IT'S AN ENUM" ~ Alf
        {
            //Pop open preview + editing window
        }
    }
    */

    public void Delete()
    {
        if (prefabEventList == null)
            return;

        prefabEventList.holders.Remove(this);
        prefabEventList.SaveAll();
        Destroy(gameObject);

    }

    public void SetEvent(AttackEvent ae, PrefabEventList manager = null)
    {
        prefabEventList = manager;
        attackEvent = ae;
        text.text = attackEvent.displayName;
        deleteButton.SetActive(prefabEventList != null);
    }
}

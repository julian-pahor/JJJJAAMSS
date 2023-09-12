using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BeatSlot : MonoBehaviour, IDropHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    //maybe put the prefab in the timeline editor and get it from there
    public BeatItem preFab;
    public AttackEventUICard uiCard;

    public Color selectedColour;
    public Color baseColour;

    [SerializeField]
    AttackEvent attackEvent; // If attackEvent is null that means this event should be dictated by a rest

    public TimelineEditor editor;

    private Vector2 clickCheck;
    private float hoverTimer = .35f;
    private bool hovering = false;

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
        editor.SelectEvent(attackEvent);
        UpdateSlot();
        
    }

    public void LoadSlotEvent(AttackEvent attackEvent)
    {
        this.attackEvent = attackEvent;
    }
    //----------------------

    //dropping a beat item on a slots sets our attack event to the one stored in the beat item
    public void OnDrop(PointerEventData eventData)
    {
       
        GameObject drop = eventData.pointerDrag;
        BeatItem draggedItem = drop.GetComponent<BeatItem>();

        if (draggedItem != null)
        {
            SetSlotEvent(Instantiate(draggedItem.thisEvent));
        }

    }

    
    //right click to clear slot
    public void OnPointerUp(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Right)
        {
            
            if(Vector2.Distance(clickCheck, eventData.position) < 15)
            {
                this.attackEvent = null;
                UpdateSlot();
            }
            
        }
    }

    //sets our uiCard to reflect the state of the slot
    public void UpdateSlot()
    {
        //THE CERBERUS OPERATOR
        bool selected = editor.eventEditor.currentlySelectedEvent == attackEvent ? true : false;

        GetComponent<Image>().color = attackEvent == null ? baseColour : selected ? selectedColour : baseColour;

        uiCard.SetDisplay(attackEvent, selected);

    }

    //create a dummy object to allow draggin between slots
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (attackEvent == null || eventData.button == PointerEventData.InputButton.Right)
            return;

        BeatItem item = Instantiate(preFab, eventData.position, Quaternion.identity);
        item.SetAttack(attackEvent);
        item.DragInitialise(transform.root);
        eventData.pointerDrag = item.gameObject;

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            attackEvent = null;
        }
        UpdateSlot();
    }

    //need these interfaces or the other ones don't work cheers thx unity
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            clickCheck = eventData.position;
            return;
        }
        //send selected event data to editor
        editor.SelectEvent(attackEvent);
    }   
    
    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(attackEvent != null)
        {
            hovering = true;
            StartCoroutine(Preview());
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        StopCoroutine(Preview());
        editor.previewPopUp.transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() => 
        { 
            editor.previewPopUp.SetActive(false); 
            editor.previewPopUp.transform.localScale = Vector3.one;
            hoverTimer = .35f;
        });

        
    }

    private IEnumerator Preview()
    {
        yield return null;

        while(hoverTimer > 0)
        {
            hoverTimer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(!hovering)
        {
            hoverTimer = .35f;
            yield break;
        }


        //Spawn Window and Fire Off Preview
        editor.previewPopUp.SetActive(true);
        editor.previewPopUp.transform.DOScale(0, 0.3f).From().SetEase(Ease.OutBack).OnComplete(() => { attackEvent.Fire(); });

        while(hovering)
        {
            yield return new WaitForSeconds(1.5f);
            attackEvent.Fire();
        }
    }
}

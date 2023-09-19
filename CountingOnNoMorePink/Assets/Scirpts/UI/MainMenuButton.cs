using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update

    public Color baseColour;
    public Color highlightColour;
    public float scalingFactor;
    public float hightlightTime;

    public UnityEvent onClick;

    float timer;
    bool mouseOver;
    Image imig;
    RectTransform rectTransform;

    private void Start()
    {
        imig = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //oh no
        if(mouseOver && timer < hightlightTime)
            timer += Time.deltaTime;
        else if(!mouseOver && timer >0)
            timer -= Time.deltaTime;

        float lerp = timer/hightlightTime;

        rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scalingFactor, lerp);
        imig.color = Color.Lerp(baseColour,highlightColour, lerp);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver=false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}

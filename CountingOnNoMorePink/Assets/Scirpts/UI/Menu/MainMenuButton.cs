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

    private FMODUnity.StudioEventEmitter blip;
    private FMODUnity.StudioEventEmitter selG;
    private FMODUnity.StudioEventEmitter selB;

    private void Start()
    {
        imig = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        blip = MenuAudioManager.instance.blipEmitter;
        selG = MenuAudioManager.instance.selGEmitter;
        selB = MenuAudioManager.instance.selBEmitter;
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

    public void ResetHighlight()
    {
        timer = 0;
        mouseOver = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        blip.Play();
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver=false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClick.GetPersistentEventCount() > 0)
        {
            selG.Play();
        }
        else
        {
            selB.Play();
        }

        onClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}

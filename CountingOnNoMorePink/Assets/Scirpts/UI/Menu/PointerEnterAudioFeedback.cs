using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEnterAudioFeedback : MonoBehaviour, IPointerEnterHandler
{
    MenuAudioManager mam;
    Button button;
    void Start()
    {
        mam = MenuAudioManager.instance;

        button = GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(mam != null)
        {
            if(button == null)
            {
                mam.blipEmitter.Play();
            }
            else if(button != null && button.interactable) //Check for runtime built level select buttons
            {
                mam.blipEmitter.Play();
            }
            else
            {

            }
        }
    }
}

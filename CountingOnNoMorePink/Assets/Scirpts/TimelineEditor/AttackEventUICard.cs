using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEditor.Build;

public class AttackEventUICard : MonoBehaviour
{

    public TextMeshProUGUI displayText;
    private Tween onTween;
   
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetDisplay(AttackEvent attackEvent, bool thisSlot)
    {

        if (attackEvent != null)
        {
            if (thisSlot)
            {
                TurnOn();
            }

            displayText.text = attackEvent.displayName;
        }
        else
        {
            TurnOff();
        }
    }

    public void LoadDisplay(AttackEvent attackEvent)
    {
        displayText.text = attackEvent.displayName;
    }

    private void TurnOn()
    {
        transform.localScale = Vector3.one / 10;
        gameObject.SetActive(true);
        onTween = transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        
    }

    private void TurnOff()
    {
        if(onTween != null)
        {
            DOTween.Kill(onTween);
        }
        
        transform.DOScale(0, 0.05f).SetEase(Ease.OutSine).OnComplete(() => { gameObject.SetActive(false); transform.localScale = Vector3.one; });

    }

    //THIS IS FOR WHEN THE SONG IS CALLED TO LOAD
    //ITS OVERLOADING WITH TWEENS
    //I WILL FIGURE THIS OUT EVENTUALLY BUT NOT RIGHT NOW
    public Tween OnLoad()
    {
        transform.localScale = Vector3.one / 10f;
        gameObject.SetActive(true);
        return transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }
}

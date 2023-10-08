using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

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

    public Tween TurnOff()
    {
        if(onTween != null)
        {
            DOTween.Kill(onTween);
        }
        
        return transform.DOScale(0, 0.05f).SetEase(Ease.OutSine).OnComplete(() => { gameObject.SetActive(false); transform.localScale = Vector3.one; });

    }

    public Tween OnLoad()
    {
        transform.localScale = Vector3.one / 10f;
        gameObject.SetActive(true);
        return transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    
}

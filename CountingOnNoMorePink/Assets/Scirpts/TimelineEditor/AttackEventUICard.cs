using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackEventUICard : MonoBehaviour
{

    public TextMeshProUGUI displayText;

    private void Start()
    {
 
    }

    public void SetDisplay(AttackEvent attackEvent)
    {
        gameObject.SetActive(attackEvent != null);
        if(attackEvent != null)
        {
            displayText.text = attackEvent.displayName;
        }
    }


}

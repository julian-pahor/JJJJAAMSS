using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    TextMeshProUGUI readout;


    // Start is called before the first frame update
    void Start()
    {
        if(player == null) player = FindObjectOfType<FreeFormOrbitalMove>();
        player.onTakeDamage += UpdateUI;
        readout = GetComponent<TextMeshProUGUI>();
    }

    void UpdateUI()
    {
        int currentHp = player.currentHP;
        int maxHp = player.maxHP;
        string hpdisplay = "";

        for (int i = 0; i < maxHp; i++)
        {
            if(currentHp >= i+1)
            {
                hpdisplay += "O";
            }
            else
            {
                hpdisplay += "X";
            }
        }
        readout.text = hpdisplay;
    }
}

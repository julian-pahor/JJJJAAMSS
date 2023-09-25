using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    public HealthPip pipPrefab;

    List<HealthPip> healthPips = new List<HealthPip>();

    void Start()
    {
        if(player == null) player = FindObjectOfType<FreeFormOrbitalMove>();
        player.onTakeDamage += UpdateUI;

        Setup();
    }


    void Setup()
    {
        for(int i = 0; i < player.maxHP; i++)
        {
            HealthPip pip = Instantiate(pipPrefab,transform);
            healthPips.Add(pip);
        }
    }

    void UpdateUI()
    {
        int currentHp = player.currentHP;
        int maxHp = player.maxHP;

        for (int i = 0; i < maxHp; i++)
        {
            if(currentHp >= i+1)
            {
                healthPips[i].SetState(true);
            }
            else
            {
                healthPips[i].SetState(false);
            }
        }
    
    }
}

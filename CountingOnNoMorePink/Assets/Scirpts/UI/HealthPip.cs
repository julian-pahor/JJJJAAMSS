using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPip : MonoBehaviour
{
  

    public Sprite fullSprite;
    public Sprite damageSprite;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }


    public void SetState(bool full)
    {
        image.sprite = full ? fullSprite : damageSprite;
    }
}

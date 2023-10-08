using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageFlash : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    Image image;
    public float flashtime;
    public float stretch;
    float timer;

    private void Start()
    {
        image = GetComponent<Image>();
        player.onTakeDamage += Flash;
    }
    void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime;

        image.color = new Color(image.color.r, image.color.g, image.color.b,Mathf.Lerp(0,1,timer/flashtime));

        float storch = Mathf.Lerp(stretch, 0, timer / flashtime);

        image.rectTransform.offsetMax = new Vector2(storch,storch);
        image.rectTransform.offsetMin = new Vector2(-storch, -storch);

    }

    void Flash()
    {
        timer = flashtime;
    }
}

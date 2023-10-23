using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverscreen : MonoBehaviour
{
    public Image splash;
    public Button retryButton;
    public float fadeTime;
    float timer;
    bool active;


    public void Activate()
    {
        timer = 0f;
        active = true;
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!active)
            return;

        if(timer < fadeTime)
            timer += Time.deltaTime;

        float lerp = timer / fadeTime;

        splash.color = new Color(splash.color.r, splash.color.g, splash.color.b,lerp);

        //retryButton.gameObject.SetActive(timer >= fadeTime);


    }
}

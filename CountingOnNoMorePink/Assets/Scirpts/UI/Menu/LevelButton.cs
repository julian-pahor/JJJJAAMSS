using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelTitle;
    
    public Color focusColour;
    public Color backgroundColour;
    public Image image;

    float transitionTime;
    float timer;
    int position;
    Vector2 targetPosition;
    Vector3 currentScale;
    Vector3 targetScale;
    RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        currentScale = Vector3.one;
        targetScale = Vector3.one;
    }

    public void Move(Vector2 targetPosition,Vector3 targetScale,float transitionTime, int position)
    {
        timer = 0f;
        currentScale = transform.localScale;
        this.targetScale = targetScale;
        this.targetPosition = targetPosition;
        this.transitionTime = transitionTime;
        this.position = position;
        SetColour(transitionTime);
    }

    public void SetImmediate(Vector2 targetPosition, Vector3 targetScale, int position)
    {
        this.position = position;
        rt.position = targetPosition;
        transform.localScale = targetScale;
        SetColour(0);
    }


    void SetColour(float time)
    {
        float lerp = (float)position / 2f; //max visible positions from level loader
        Color colour = Color.Lerp(focusColour, backgroundColour, lerp);
        image.DOColor(colour, time);
    }

    public void Update()
    {
        if (timer < transitionTime)
        {
            timer += Time.deltaTime;

            rt.position = Vector2.Lerp(rt.position, targetPosition, timer / transitionTime);
            transform.localScale = Vector3.Lerp(currentScale, targetScale, timer / transitionTime);
        }
    }
}

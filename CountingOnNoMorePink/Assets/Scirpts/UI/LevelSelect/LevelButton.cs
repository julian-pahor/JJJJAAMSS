using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelTitle;
    public Vector2 targetPosition;
    public Vector3 currentScale;
    public Vector3 targetScale;

    float transitionTime;
    float timer;

    RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        currentScale = Vector3.one;
        targetScale = Vector3.one;
    }

    public void Move(Vector2 targetPosition,Vector3 targetScale,float transitionTime)
    {
        timer = 0f;
        currentScale = transform.localScale;
        this.targetScale = targetScale;
        this.targetPosition = targetPosition;
        this.transitionTime = transitionTime;
    }

    public void SetImmediate(Vector2 targetPosition, Vector3 targetScale)
    {
        rt.anchoredPosition = targetPosition;
        transform.localScale = targetScale;
    }

    public void Update()
    {
        if (timer < transitionTime)
        {
            timer += Time.deltaTime;

            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetPosition, timer / transitionTime);
            transform.localScale = Vector3.Lerp(currentScale, targetScale, timer / transitionTime);
        }
    }
}

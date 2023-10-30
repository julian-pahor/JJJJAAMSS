using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Vector2 initialPosition;
    RectTransform rt;
    public float magnitude;
    void Start()
    {
      
        rt = GetComponent<RectTransform>();
        initialPosition = rt.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 target = initialPosition + (mouse*magnitude);
        rt.anchoredPosition = target;    

       
    }
}
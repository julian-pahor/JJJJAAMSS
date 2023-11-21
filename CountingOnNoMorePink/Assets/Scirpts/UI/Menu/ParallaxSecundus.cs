using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSecundus : MonoBehaviour
{
    Vector3 initialPosition;
    public float magnitude;
    void Start()
    {

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouse = new Vector3(mouse.x, mouse.y, 0);
        Vector3 target = initialPosition + (mouse * magnitude);
        transform.position = target;


    }
}

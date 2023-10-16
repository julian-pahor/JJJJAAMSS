using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryIndicator : MonoBehaviour
{

    public Polygon polygon;
    public float rotationValue;
    int index;
    float currentRotation;
    Transform player;
    public void Setup(int beats, Transform player)
    {
        index = beats;
        this.player = player;
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;
        
    }

    private void Update()
    {
        transform.position = player.position;
    }
    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeat;
    }

    void OnBeat(int beat, int bar, string marker)
    {
        currentRotation += rotationValue;

        transform.rotation = Quaternion.Euler(transform.rotation.x, currentRotation, transform.rotation.y);
        polygon.polygonRadius -= 1;
        polygon.centerRadius -= 1;
        index -= 1;
        if (index <= 0)
            Destroy(gameObject);
    }
}

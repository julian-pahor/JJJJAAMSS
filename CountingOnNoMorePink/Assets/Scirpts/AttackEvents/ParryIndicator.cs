using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryIndicator : MonoBehaviour
{
    public float rotationValue;
    int index;
    float currentRotation;
    Transform player;
    public Transform indicator;
    int totalBeats;
    Vector3 baseScale;
    public void Setup(int beats, Transform player)
    {
        index = beats;
        totalBeats = beats;
        this.player = player;
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;
        baseScale = indicator.localScale;
        
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
        index -= 1;

        transform.rotation = Quaternion.Euler(transform.rotation.x, currentRotation, transform.rotation.y);
        indicator.localScale = baseScale * ((float)index / (float)totalBeats);

   
    
        if (index <= 0)
            Destroy(gameObject);
    }
}

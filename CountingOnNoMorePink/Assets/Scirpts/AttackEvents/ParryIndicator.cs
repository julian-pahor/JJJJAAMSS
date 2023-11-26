using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryIndicator : MonoBehaviour
{

    int index;
    Transform player;

    public Color cold;
    public Color hot;

    public SpriteRenderer inner;
    public SpriteRenderer middle;
    public SpriteRenderer outer;

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
        index -= 1;

       

   
    
        if (index <= 0)
            Destroy(gameObject);
    }
}

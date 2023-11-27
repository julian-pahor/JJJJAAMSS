using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParryIndicator : MonoBehaviour
{

    int index;
    Transform player;

    public Color inactive;
    public Color active;

    public SpriteRenderer inner;
    public SpriteRenderer middle;
    public SpriteRenderer outer;

    public float flashTime;
    float flash;

    public AnimationCurve flashCurve;
    public void Setup(int beats, Transform player)
    {
        index = beats;
    
        this.player = player;
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;
        DoBeat();
        
    }

    private void Update()
    {
        transform.position = player.position;
        if (flash <= 0)
            return;

        flash -= Time.deltaTime;
        inner.color = Color.Lerp(active,Color.white,flashCurve.Evaluate(flash/flashTime));
        
    }
    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeat;
    }

    void OnBeat(int beat, int bar, string marker)
    {
       DoBeat();
    }
    void DoBeat()
    {
     
        switch (index)
        {
            case 3:
                inner.color = inactive;
                middle.color = inactive;
                outer.color = active;
    
                break;

            case 2:
                inner.color = inactive;
                middle.color = active;
                outer.color = inactive;
          
                break;

            case 1:
                inner.color = active;
                middle.color = inactive;
                outer.color = inactive;
                flash = flashTime;
                break;
        }

        index -= 1;

        if (index < 0)
        {
            Destroy(gameObject);
        }
    }
}

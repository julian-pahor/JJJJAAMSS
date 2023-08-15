using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTimeline : MonoBehaviour
{
    public float beatLength;
    public List<AttackEvent> attackEvents; //make this list of lists to enable multiple attackevents per beat
    public GameObject win;

    float timer;
    int index;

    //private void Update()
    //{
    //    if (attackEvents.Count == 0)
    //        return;

    //    timer += Time.deltaTime;

    //    if (timer >= beatLength)
    //    {

    //        timer = 0;
    //        attackEvents[index].Fire();
    //        index++;

    //        if (index >= attackEvents.Count)
    //        {
    //            index = 0;
    //        }

    //    }
    //}


    private void Start()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Beat;
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Beat;
    }

    private void Beat(int bar, int beat)
    {
        attackEvents[index].Fire();
        index++;

        if (index >= attackEvents.Count)
        {
            index = 0;
        }
    }

}

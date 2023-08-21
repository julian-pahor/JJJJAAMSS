using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParryTester : MonoBehaviour
{
    public Parry player;
    

    private void Awake()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Beat;
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Beat;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Beat(int m, int b)
    {
        if(b == 4)
        {
            player.ParryChance();
        }
    }
}

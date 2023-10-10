using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTestingZone : MonoBehaviour
{

    public Parry parry;

    //PreFabs
    public GameObject beatIndicator;
    public GameObject inputIndicator;



    // Start is called before the first frame update
    void Start()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += BeatReader;
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= BeatReader;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeatReader(int m, int b)
    {

    }
}

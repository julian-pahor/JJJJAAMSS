using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttack2 : MonoBehaviour
{

    private int index = 0;

    private Vector3 start;
    private Vector3 mid;
    private Vector3 end;

    private float currentLerp;


    private float currentTravel = 0;
    private float travelTime = 0.462f;

    public Transform trail;

    // Start is called before the first frame update
    void Start()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;
        int direction = Random.Range(0,2);
        //I (Julian), hereby apologise to Alf and any future programmer reading this next line of code
        start = direction== 1 ? Wobbit.instance.hand1.position : Wobbit.instance.hand2.position;
        mid = direction == 1 ? Wobbit.instance.anchor1.position : Wobbit.instance.anchor2.position;
        end = Wobbit.instance.player.position;
        transform.position = start;
       
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(index > 0)
        {
            currentLerp = currentTravel / travelTime;
            transform.position = Utilities.QuadraticLerp(start, mid, end, currentLerp);
            currentTravel += Time.deltaTime;
        }

        end = Wobbit.instance.player.position;

        if(currentLerp > 1)
        {
            if (trail != null)
                trail.transform.SetParent(null);
            //currently destroying itself at end of quad lerp
            Destroy(this.gameObject);
        }
    }

    public void OnBeat(int m, int b, string marker)
    {
        index++;

    }
}

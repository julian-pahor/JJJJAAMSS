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

    // Start is called before the first frame update
    void Start()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;

        //I (Julian), hereby apologise to Alf and any future programmer reading this next line of code
        start = Random.Range(0, 2) == 1 ? Wobbit.instance.hand1.position : Wobbit.instance.hand2.position;
        end = Wobbit.instance.player.position;
        mid = end - start;
        mid = mid.normalized;
        mid = (mid * 2f) + (Vector3.up * 3f);
        
        transform.position = start;
        transform.DOPunchScale(Vector3.one * 1.25f, 0.175f);
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left * 50f * Time.deltaTime);

        if(index > 0)
        {
            currentLerp = currentTravel / travelTime;
            transform.position = Utilities.QuadraticLerp(start, mid, end, currentLerp);
            currentTravel += Time.deltaTime;
        }

        end = Wobbit.instance.player.position;

        if(currentLerp > 1)
        {
            //currently destroying itself at end of quad lerp
            Destroy(this.gameObject);
        }
    }

    public void OnBeat(int m, int b)
    {
        index++;
        if(index == 1)
        {
            transform.DOPunchScale(Vector3.one * 1.3f, 0.2f);
        }
    }
}

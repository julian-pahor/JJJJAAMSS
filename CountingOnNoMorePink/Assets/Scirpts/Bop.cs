using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bop : MonoBehaviour
{
    float beatLength;

    Vector3 startRot;
    public Vector3 endRot;

    public AnimationCurve bop;
    bool isBop;
    void Start()
    {
        startRot = transform.rotation.eulerAngles;
        beatLength = BeatBroadcast.instance.beatLength/2;
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += BopBegins;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isBop) return;

        float bopQuotient = Mathf.PingPong(Time.time, beatLength);
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRot),Quaternion.Euler(endRot), bop.Evaluate(bopQuotient/beatLength));


    }

    void BopBegins(int a, int b)
    {
        if(isBop) return;
        isBop = true;
    }
}

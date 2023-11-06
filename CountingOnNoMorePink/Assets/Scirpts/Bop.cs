using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bop : MonoBehaviour
{
    public AnimationCurve bop;

    [Range(1f, 10f)]
    public float bopMagnitude;
    public bool offbeat;

    float beatLength;
    float timer;

    Vector3 startScale;
    Vector3 endScale;
    
    bool isBop;
    void Start()
    {
        startScale = new Vector3(transform.localScale.x * bopMagnitude, transform.localScale.y, transform.localScale.z * bopMagnitude);
        endScale = new Vector3 (transform.localScale.x, transform.localScale.y * bopMagnitude, transform.localScale.z);
        beatLength = BeatBroadcast.instance.beatLength;
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += BopBegins;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > beatLength)
            timer = 0;

        if (!isBop) return;
        float bopQuotient = bop.Evaluate(timer/beatLength);

        if (offbeat)
            bopQuotient = 1 - bopQuotient;

        transform.localScale = Vector3.Lerp(startScale, endScale, bopQuotient);  

    }

    void BopBegins(int a, int b, string marker)
    {
        if(isBop) return;
        isBop = true;
    }
}

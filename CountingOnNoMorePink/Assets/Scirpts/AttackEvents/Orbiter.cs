using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    Transform origin;
    int lifetime;
    float speed;
    int direction;
    float angle;
    float angleOffset;
    float distance;

    bool bop;

    Rigidbody rb;

    public void Initialise(int lifetime, float speed, int direction, float offset)

    {
        rb = GetComponent<Rigidbody>();
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;
        origin = Wobbit.instance.bossOrigin;
        this.lifetime = lifetime;
        angleOffset = offset;
        distance = Vector3.Distance(transform.position, origin.position);
        this.speed = speed;
        this.direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * direction * Time.deltaTime;
       

    }

    private void FixedUpdate()
    {
        //rb.rotation = Quaternion.LookRotation(transform.position - origin.transform.position);
        rb.MovePosition(Utilities.PointWithPolarOffset(origin.position, distance, angle + angleOffset));
    }

    void OnBeat(int measure, int beat)
    {
        lifetime -= 1;
        float bopindex = bop ? 1f : 1.5f;
        transform.localScale = Vector3.one * bopindex;
        bop = !bop;

        if (lifetime <= 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeat;
    }
}

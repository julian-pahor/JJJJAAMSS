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
    float bopIndex;
    float bopTimer;

    Rigidbody rb;

    public void Initialise(int lifetime, float speed, int direction, float offset, float bopIndex)

    {
        rb = GetComponent<Rigidbody>();
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;
        origin = Wobbit.instance.bossOrigin;
        this.lifetime = lifetime;
        angleOffset = offset;
        distance = Vector3.Distance(transform.position, origin.position);
        this.speed = speed;
        this.direction = direction;
        this.bopIndex = bopIndex;
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * direction * Time.deltaTime;
        bopTimer += Time.deltaTime;
        float lerp = bopTimer/bopIndex;
        if(lerp < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.one * 2f, Vector3.one,lerp);
        }

    }

    private void FixedUpdate()
    {
        //rb.rotation = Quaternion.LookRotation(transform.position - origin.transform.position);
        rb.MovePosition(Utilities.PointWithPolarOffset(origin.position, distance, angle + angleOffset));
    }

    void OnBeat(int measure, int beat)
    {
        lifetime -= 1;
        
        //transform.localScale = Vector3.one * 1.5f;
        bopTimer = 0;

        if (lifetime <= 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeat;
    }
}

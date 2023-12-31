using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Orbiter : MonoBehaviour
{
    public Material baseMat;
    public Material activeMat;
    public VisualEffect vfx;
    public MeshRenderer renenderer;

    Transform origin;

    float speed;
    int direction;
    float angle;
    float angleOffset;//offset of 
    float distance;

    //these are measured in beats
    int duration;
    int warmup;

    bool isActive;
    bool isDestroyed;

    //boppin
    float bopIndex;
    float bopTimer;

    Rigidbody rb;
    Collider col;

    bool wasPreviouslyInitialised;

    public void Initialise(int lifetime, float speed, int direction, float offset, float bopIndex, int warmup)

    {   isActive = false;
        isDestroyed = false;
        bopTimer = 0f;
        GetComponentInChildren<Renderer>().enabled = true;
        vfx.Stop();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        col.enabled = false;

        if (!wasPreviouslyInitialised)
        {
            wasPreviouslyInitialised = true;
            BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeat;
        }

        renenderer.material = baseMat;
        origin = Wobbit.instance.bossOrigin;
        this.duration = lifetime;
        angleOffset = offset;
        distance = Vector3.Distance(transform.position, origin.position);
        this.speed = speed;
        this.direction = direction;
        this.bopIndex = bopIndex;
        this.warmup = warmup;

    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * direction * Time.deltaTime;
        bopTimer += Time.deltaTime;

        if (!isDestroyed)
        {
            //bop while active
            if (isActive)
            {

                float lerp = bopTimer / bopIndex;
                if (lerp < 1)
                {
                    transform.localScale = Vector3.Lerp(Vector3.one * 2f, Vector3.one, lerp);
                }
            }
            else
            {
                float halfBop = BeatBroadcast.instance.beatLength / 2;
                float lerp = Mathf.PingPong(bopTimer, halfBop) / halfBop;
                transform.localScale = Vector3.Lerp(Vector3.one * 0.01f, Vector3.one, lerp);
            }
        }
        else
        {
            float lerp = bopTimer / BeatBroadcast.instance.beatLength;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.01f, lerp);
            if(lerp >= 0.9f)
            {
                GetComponent<PooledObject>().Despawn();
            }

        }
    }

    private void FixedUpdate()
    {
        //rb.rotation = Quaternion.LookRotation(transform.position - origin.transform.position);
        rb.MovePosition(Utilities.PointWithPolarOffset(origin.position, distance, angle + angleOffset));
    }

    void OnBeat(int measure, int beat, string marker)
    {
        if (isActive)
        {
            duration -= 1;

            //transform.localScale = Vector3.one * 1.5f;
            bopTimer = 0;

            if (duration <= 0)
            {
                bopTimer = 0;
                isDestroyed = true;
                col.enabled = false;
            }
        }
        else
        {
            warmup -= 1;
            if(warmup <= 0)
            {
                Activate();
            }
        }
    }


    void Activate()
    {
        isActive = true;
        bopTimer = 0;
        col.enabled = true;
        renenderer.material = activeMat;
        vfx.Play();
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeat;
    }
}

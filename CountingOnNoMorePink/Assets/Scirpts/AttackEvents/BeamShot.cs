
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/BeamShot")]
public class BeamShot : AttackEvent
{

    public int beams;
    public float segments;
    public float firingArc;
    public float distance;
    public float minDistance;
    public float arcOffset;
    public float delay;

    public int duration;


    public BeamType type;
    public BeamBehaviour behaviour;

    public float arcStep;

    public DelayedDangerZone effectsPrefab;

    public Artillery artilleryEffect;

    //phase out this stuff
    public LineTracer lineTracer;
    LineTracer tempTracer;

    public enum BeamType { Instant,RadiateInward,RadiateOutward}
    public enum BeamBehaviour {BetweenBeats, BeatToBeat}

    public override void Fire()
    {
        base.Fire();

        Transform origin = Wobbit.instance.bossOrigin;

        if (beams <= 0) { beams = 1; } //stop it dividing by 0

        float angleStep = (firingArc / beams); //divide total arc of fire by number of shots

        //launch vector
        Vector3 launchvector = Utilities.PointWithPolarOffset(origin.position, 1, arcOffset) - origin.position;
        launchvector = launchvector.normalized;


        float rotation = (float)Mathf.Atan2(launchvector.x, launchvector.z) * 180 / Mathf.PI;


        if (rotation < 0) { rotation = 360 + rotation; } //fix negative degrees


        //instantiate at distance from origin

        for (var i = 0; i < beams; ++i)
        {
            //create tracer if one has been assigned
            if(lineTracer != null)
            {
                tempTracer = Instantiate(lineTracer, origin.position, Quaternion.identity);
                tempTracer.waypoints.Add(origin.position);
            }
            
            float rotationO = rotation;
            rotationO = rotation - firingArc / 2 + angleStep / 2;
            rotationO += angleStep * i;

            float increment = distance / segments;
            for(int j = 0; j < segments; ++j)
            {
                float dist = increment * j;
                Vector3 point = Utilities.PointWithPolarOffset(origin.position, dist + minDistance, rotationO + (arcStep * j));
            

                if (effectsPrefab == null) effectsPrefab = Wobbit.instance.delayedDangerZoneTest;

                DelayedDangerZone delayedZone = Wobbit.instance.poolPool.spikePool.Spawn().GetComponent<DelayedDangerZone>();
                delayedZone.transform.position = point;

                //assign waypoints to tracer (revisit this)
                if (lineTracer != null)
                {
                    tempTracer.waypoints.Add(point);
                }

                if (artilleryEffect != null)
                {
                    delayedZone.SetArtilleryTracer(artilleryEffect);
                }
                                
                switch(behaviour)
                {
                    case BeamBehaviour.BeatToBeat:
                        OnBeat(delayedZone, j);

                        break;
                    case BeamBehaviour.BetweenBeats:
                        OnTimer(delayedZone, j);
                        break;
                }

                  //assign waypoints to tracer (revisit this)
                if (lineTracer != null)
                {
                    tempTracer.Initialise(BeatBroadcast.instance.beatLength * duration, 2);
                }



               

            }

         

        }

        void OnTimer(DelayedDangerZone dd, int index)
        {
            if (duration <= 0) duration = 1; //min duration of 1 beat (maybe change later)

            //multiply a single beatlength by the duration in beats, divide by segments to get time for each segment
            float popTime = (BeatBroadcast.instance.beatLength * duration) / segments;

            float pop = popTime;

            if (type == BeamType.RadiateOutward) { pop = popTime * index; }

            if (type == BeamType.RadiateInward)
            {
                pop = (popTime * segments) - (popTime * index);
                if (lineTracer != null)
                {
                    tempTracer.waypoints.Reverse();
                }
            }

            dd.InitialiseOnTimer(pop ,popTime + delay, popTime);
          
        }

        void OnBeat(DelayedDangerZone dd, int index)
        {
            int pop = 1;

            if (type == BeamType.RadiateOutward) { pop = index;}

            if (type == BeamType.RadiateInward)
            {
                pop = (int)segments - index;
                if (lineTracer != null)
                {
                    tempTracer.waypoints.Reverse();
                }
            }

            dd.InitialiseOnBeat(pop, 1);
        }

    }

    public override void HookUp(EventEditor eventEditor)
    {
        ValueEditor valueEditor;

        //Set Up Beam Amount
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { beams = (int)f; }, beams, "Beams");

        //Set Up Arc offset
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { arcOffset = f; }, arcOffset, "Arc Offset");

        //Set Up Delay
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { delay = f; }, delay, "Delay",0,10);


        //Inner Radius
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { minDistance = f; }, minDistance, "Inner Radius");

        //Outer Radius
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { distance = f; }, distance, "Beam Length");

        //Beam Segments
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { segments = f; }, segments, "Beam Segments");

        //Duration
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { duration = (int)f; }, duration, "Duration");

        //SpiralStep;
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { arcStep = f; }, arcStep, "Spiral Step",-360f,360f);
    }
}

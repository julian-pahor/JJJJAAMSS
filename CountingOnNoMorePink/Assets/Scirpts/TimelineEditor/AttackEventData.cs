using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEventData
{
    //class name
    public string attackEventID;
    public string fileName;

    //beams and bullets
    public int shots;
    public float shotSpeed;
    public float offset;
    public float firingArc;

    public float delay;

    public float innerRadius;
    public float outerRadius;

    public float spiralStep;

    public SpreadShot.ShotType shotType;

    //this might need to move so it applies to bullets too
    public BeamShot.BeamBehaviour behaviour;
    public BeamShot.BeamType radiateStyle;
    public float beamSegments; //change to int??

    public int duration; // = lifetime for orbiters

    public float orbitalSpeed;
    public int orbitalDirection;

    public ParryEvent.ParryType parryType;

    public AttackEventData()
    {
        attackEventID = "null";
    }

    public void SerialiseAsData(AttackEvent attackEvent)
    {
        if (attackEvent == null)
        {
            attackEventID = "null";
            return;
        }

        //make scrobject into datable object
        attackEventID = attackEvent.GetType().Name;
        fileName = attackEvent.displayName;

        //do not question me
        switch(attackEvent)
        {
            case SpreadShot ss:

                shots = ss.shots;
                shotSpeed = ss.bulletSpeed;
                offset = ss.arcOffset;
                firingArc = ss.firingArc;
                shotType = ss.shotType;

                break;

            case BeamShot bs:

                shots = bs.beams;
                offset = bs.arcOffset;
                firingArc = bs.firingArc;

                innerRadius = bs.minDistance;
                outerRadius = bs.distance;

                delay = bs.delay;

                beamSegments = bs.segments;

                duration = bs.duration;

                behaviour = bs.behaviour;
                radiateStyle = bs.type;

                spiralStep = bs.arcStep;

                break;


            case OrbiterAttack oa:

                shots = oa.beams;
                offset = oa.arcOffset;
                firingArc = oa.firingArc;

                innerRadius = oa.minDistance;
                outerRadius = oa.distance;

                beamSegments = oa.segments;

                duration = oa.lifeTime;

                orbitalSpeed = oa.speed;
                orbitalDirection = oa.direction;

                spiralStep = oa.arcStep;


                break;
            case Seeker s:

                delay = s.delay;

                break;
            case ParryEvent ps:
                parryType = ps.type;
                break;
        }

    }

    public void DeserialiseIntoObject(AttackEvent attackEvent)
    {

        if (attackEvent == null)
            return;

        attackEvent.displayName = fileName;

        //DO NOT QUESTION ME
        switch (attackEvent)
        {
            case SpreadShot ss:

                ss.shots = shots;
                ss.bulletSpeed = shotSpeed;
                ss.arcOffset = offset;
                ss.firingArc = firingArc;
                ss.shotType = shotType;
                break;

            case BeamShot bs:

                bs.beams = shots;
                bs.arcOffset = offset;
                bs.firingArc = firingArc;

                bs.delay = delay;

                bs.minDistance = innerRadius;
                bs.distance = outerRadius;

                bs.segments = beamSegments;

                bs.duration = duration;

                bs.behaviour = behaviour;
                bs.type = radiateStyle;

                bs.arcStep = spiralStep;

                break;


            case OrbiterAttack oa:

                oa.beams = shots;
                oa.arcOffset = offset;
                oa.firingArc = firingArc;

                oa.minDistance = innerRadius;
                oa.distance = outerRadius;

                oa.segments = beamSegments;

                oa.lifeTime = duration;

                oa.speed = orbitalSpeed;
                oa.direction = orbitalDirection;

                oa.arcStep = spiralStep;


                break;
            case Seeker s:

                s.delay = delay;

                break;
            case ParryEvent ps:
                break;
            default:
                Debug.Log("There was a serious issue");
                break;
        }

    }
}
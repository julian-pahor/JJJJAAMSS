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
    public float offset;

    public float innerRadius;
    public float outerRadius;

    public float spiralStep;

    //this might need to move so it applies to bullets too
    public BeamShot.BeamBehaviour behaviour;
    public BeamShot.BeamType radiateStyle;
    public float beamSegments; //change to int??

    public int duration; // = lifetime for orbiters

    public float orbitalSpeed;
    public int orbitalDirection;

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
        fileName = attackEvent.name;

        //do not question me
        switch(attackEvent)
        {
            case SpreadShot ss:

                shots = ss.shots;
                offset = ss.arcOffset;

                break;

            case BeamShot bs:

                shots = bs.beams;
                offset = bs.arcOffset;

                innerRadius = bs.minDistance;
                outerRadius = bs.distance;

                beamSegments = bs.segments;

                duration = bs.duration;

                behaviour = bs.behaviour;
                radiateStyle = bs.type;

                spiralStep = bs.arcStep;

                break;


            case OrbiterAttack oa:

                shots = oa.beams;
                offset = oa.arcOffset;

                innerRadius = oa.minDistance;
                outerRadius = oa.distance;

                beamSegments = oa.segments;

                duration = oa.lifeTime;

                orbitalSpeed = oa.speed;
                orbitalDirection = oa.direction;

                spiralStep = oa.arcStep;


                break;
            case Seeker s:

                //nothing really to save hey
                //Hello!

                break;
        }

    }

    public void DeserialiseIntoObject(AttackEvent attackEvent)
    {

        if (attackEvent == null)
            return;

        attackEvent.name = fileName;

        //DO NOT QUESTION ME
        switch (attackEvent)
        {
            case SpreadShot ss:

                ss.shots = shots;
                ss.arcOffset = offset;

                break;

            case BeamShot bs:

                bs.beams = shots;
                bs.arcOffset = offset;

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

                oa.minDistance = innerRadius;
                oa.distance = outerRadius;

                oa.segments = beamSegments;

                oa.lifeTime = duration;

                oa.speed = orbitalSpeed;
                oa.direction = orbitalDirection;

                oa.arcStep = spiralStep;


                break;
            case Seeker s:

                //nothing really to save hey

                break;
            default:
                Debug.Log("There was a serious issue");
                break;
        }

    }
}
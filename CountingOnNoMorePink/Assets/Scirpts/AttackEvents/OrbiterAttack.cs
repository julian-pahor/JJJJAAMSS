using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/OrbiterAttack")]
public class OrbiterAttack : AttackEvent
{

    public int beams;
    public float segments;
    public float firingArc;
    public float distance;
    public float minDistance;
    public float arcOffset;
    public int lifeTime;
    public int direction;
    public float speed;
    public BeamType type;



    public float arcStep;

    public enum BeamType { Instant, RadiateInward, RadiateOutward }

    public override void Fire()
    {
        base.Fire();

        if (minDistance <= 0) minDistance = 1; //can't rotate around an exact center

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


            float rotationO = rotation;
            rotationO = rotation - firingArc / 2 + angleStep / 2;
            rotationO += angleStep * i;

            float increment = distance / segments;
            for (int j = 0; j < segments; ++j)
            {
              
                    float dist = increment * j;
                    Vector3 point = Utilities.PointWithPolarOffset(origin.position, dist + minDistance, rotationO + (arcStep * j));
                    Orbiter o = Instantiate(Wobbit.instance.orbiterPrefab, point, Quaternion.identity);

                    o.Initialise(lifeTime, speed, direction,rotationO + (arcStep * j), (BeatBroadcast.instance.beatLength/segments) *(j+1),2);
                

                //float pop = popTime;

                //if (type == BeamType.RadiateOutward) { pop = popTime * j; }

                //if (type == BeamType.RadiateInward)
                //{
                //    pop = (popTime * segments) - (popTime * j);
                //    if (lineTracer != null)
                //    {
                //        tempTracer.waypoints.Reverse();
                //    }
                //}
            }
        }
    }

    public override void HookUp(EventEditor ee)
    {
        ValueEditor ve;

        //Beams
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { beams = (int)f; }, beams, "Beams", 1, 360, true);

        //Offset
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { arcOffset = f; }, arcOffset, "Arc Offset", 0, 360);

        //Inner Radius
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { minDistance = f; }, minDistance, "Inner Radius");

        //Outer Radius
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { distance = f; }, distance, "Beam Length");

        //Beam Segments
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { segments = f; }, segments, "Beam Segments");

        //Duration
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { lifeTime = (int)f; }, lifeTime, "Duration");

        //Orbital Speed
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { speed = f; }, speed, "Orbital Speed");

        //Orbital Direction
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { direction = (int)f; }, direction, "Orbital Direction", -1, 1, true);

        //SpiralStep;
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { arcStep = f; }, arcStep, "Spiral Step", -360f, 360f);
    }
}

